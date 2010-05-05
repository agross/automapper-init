using AutoMapper;

using Castle.Windsor;

namespace AutoMapper_Init.Infrastructure.AutoMapperIntegration
{
	public class AutoMapperInitializer : IRequireConfigurationOnStartup
	{
		readonly IConfiguration _configuration;
		readonly IConfigurationProvider _configurationProvider;
		readonly IWindsorContainer _serviceLocator;

		public AutoMapperInitializer(IConfigurationProvider configurationProvider,
									 IConfiguration configuration,
									 IWindsorContainer serviceLocator)
		{
			_configurationProvider = configurationProvider;
			_configuration = configuration;
			_serviceLocator = serviceLocator;
		}

		public IMapper[] Mappers
		{
			get;
			set;
		}

		public void Configure()
		{
			_configuration.ConstructServicesUsing(x => _serviceLocator.Resolve(x));
			Mappers.Each(x => x.RegisterMapping(_configuration));

			_configurationProvider.AssertConfigurationIsValid();
		}
	}

}