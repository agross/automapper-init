require 'rake/clean'
require 'configatron'
require 'dictionary'
Dir.glob(File.join(File.dirname(__FILE__), 'tools/Rake/*.rb')).each do |f|
	require f
end

task :default => [:clobber, 'compile:all', 'tests:run']

desc 'Runs a quick build just compiling the libs that are not up to date'
task :quick do
	CLOBBER.clear
	
	class MSBuild
		class << self
			alias do_compile compile
		end

		def self.compile(attributes)
			artifacts = artifacts_of attributes[:project]
			do_compile attributes unless uptodate? artifacts, FileList.new("#{attributes[:project].dirname}/**/*.*")
		end
		
		def self.artifacts_of(project)
			FileList.new() \
				.include("#{configatron.dir.build}/**/#{project.dirname.name}.dll") \
				.include("#{configatron.dir.build}/**/#{project.dirname.name}.exe")
		end
		
		def self.uptodate?(new_list, old_list)
			return false if new_list.empty?
			
			new_list.each do |new|
				return false unless FileUtils.uptodate? new, old_list
			end
			
			return true
		end
	end
end

namespace :env do
	Rake::EnvTask.new do |env|
		env.configure_from = 'properties.yml'
		env.configure_environments_with = lambda do |env_name|
			configure_env_for env_name
		end
	end

	def configure_env_for(env_key)
		env_key = env_key || 'development'

		puts "Loading settings for the '#{env_key}' environment"
		
		yaml = Configuration.load_yaml 'properties.yml', :hash => env_key, :inherit => :default_to, :override_with => :local_properties
		configatron.configure_from_hash yaml
		
		CLEAN.clear
		CLEAN.include('teamcity-info.xml')
		CLEAN.include('**/obj'.in(configatron.dir.source))
		CLEAN.include('**/*'.in(configatron.dir.test_results))
				
		CLOBBER.clear
		CLOBBER.include(configatron.dir.build)
		CLOBBER.include('**/bin'.in(configatron.dir.source))
		CLOBBER.include('**/*.template'.in(configatron.dir.source))
		# Clean template results.
		CLOBBER.map! do |f|
			next f.ext() if f.pathmap('%x') == '.template'
			f
		end
		
		configatron.protect_all!

		puts configatron.inspect
	end

	# Load the default environment configuration if no environment is passed on the command line.
	Rake::Task['env:development'].invoke \
		if not Rake.application.options.show_tasks and
		   not Rake.application.options.show_prereqs and
		   not Rake.application.top_level_tasks.any? do |t|
			/^env:/.match(t)
		end
end
	
namespace :generate do
	desc 'Updates the version information for the build'
	task :version do
		next if configatron.build.number.nil?
		
		asmInfo = AssemblyInfoBuilder.new({
				:AssemblyFileVersion => configatron.build.number,
				:AssemblyVersion => configatron.build.number,
				:AssemblyInformationalVersion => configatron.build.number,
				:AssemblyDescription => "#{configatron.build.number} / #{configatron.build.commit_sha}",
			})
			
		asmInfo.write 'VersionInfo.cs'.in(configatron.dir.source)
	end

	desc 'Updates the configuration files for the build'
	task :config do
		FileList.new("#{configatron.dir.source}/**/*.template").each do |template|
			QuickTemplate.new(template).exec(configatron)
		end
	end
end

namespace :compile do
	desc 'Compiles the application'
	task :app => [:clobber, 'generate:version', 'generate:config'] do
		FileList.new("#{configatron.dir.app}/**/*.csproj").each do |project|
			MSBuild.compile \
				:project => project,
				:properties => {
					:SolutionDir => configatron.dir.source.to_absolute.chomp('/').concat('/').escape,
					:Configuration => configatron.build.configuration,
					:TreatWarningsAsErrors => true
				}
		end
	end

	desc 'Compiles the tests'
	task :tests => [:clobber, 'generate:version', 'generate:config'] do
		FileList.new("#{configatron.dir.test}/**/*.Tests.csproj").each do |project|
			MSBuild.compile \
				:project => project,
				:properties => {
					:SolutionDir => configatron.dir.source.to_absolute.chomp('/').concat('/').escape,
					:Configuration => configatron.build.configuration
				}
		end
	end
	
	task :all => [:app, :tests]
end

namespace :tests do
	desc 'Runs unit tests'
	task :run => ['compile:tests'] do
		FileList.new("#{configatron.dir.build}/Tests/**/*.Tests.dll").each do |assembly|
			Mspec.run \
				:tool => configatron.tools.mspec,
				:reportdirectory => configatron.dir.test_results,
				:assembly => assembly
		end
	end
	
	desc 'Runs integation tests'
	task :integration => ['compile:tests'] do
		FileList.new("#{configatron.dir.build}/IntegrationTests/**/*.Integration.Tests.dll").each do |assembly|
			Mspec.run \
				:tool => configatron.tools.mspec,
				:reportdirectory => configatron.dir.test_results,
				:assembly => assembly
		end
	end
end