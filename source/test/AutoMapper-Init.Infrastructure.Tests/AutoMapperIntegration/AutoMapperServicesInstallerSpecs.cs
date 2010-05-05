using AutoMapper;

using AutoMapper_Init.Infrastructure.AutoMapperIntegration;

using Castle.Facilities.FactorySupport;
using Castle.MicroKernel;
using Castle.Windsor;

using Machine.Specifications;

using Rhino.Mocks;

namespace AutoMapper_Init.Infrastructure.Tests.AutoMapperIntegration
{
	[Subject(typeof(AutoMapperServicesInstaller))]
	public class When_the_auto_mapper_components_are_registered
	{
		static AutoMapperServicesInstaller Installer;
		static IWindsorContainer Container;

		Establish context = () =>
			{
				Installer = new AutoMapperServicesInstaller();
				
				Container = MockRepository.GenerateStub<IWindsorContainer>();
				Container
					.Stub(x => x.Kernel)
					.Return(MockRepository.GenerateStub<IKernel>());
				Container.Kernel
					.Stub(x => x.GetFacilities())
					.Return(new IFacility[] { });
			};

		Because of = () => Installer.Install(Container, null);

		It should_register_factory_support =
			() => Container.AssertWasCalled(x => x.AddFacility<FactorySupportFacility>());
	}
	
	[Subject(typeof(AutoMapperServicesInstaller))]
	public class When_the_auto_mapper_components_are_registered_and_factory_support_is_already_registered
	{
		static AutoMapperServicesInstaller Installer;
		static IWindsorContainer Container;

		Establish context = () =>
			{
				Installer = new AutoMapperServicesInstaller();
				
				Container = MockRepository.GenerateStub<IWindsorContainer>();
				Container
					.Stub(x => x.Kernel)
					.Return(MockRepository.GenerateStub<IKernel>());
				Container.Kernel
					.Stub(x => x.GetFacilities())
					.Return(new IFacility[] { new FactorySupportFacility() });
			};

		Because of = () => Installer.Install(Container, null);

		It should_not_register_factory_support_again =
			() => Container.AssertWasNotCalled(x => x.AddFacility<FactorySupportFacility>());
	}

	[Subject(typeof(AutoMapperServicesInstaller))]
	public class When_auto_mapper_container_registrations_are_inspected
	{
		static AutoMapperServicesInstaller Installer;
		static IWindsorContainer Container;

		Establish context = () =>
			{
				Installer = new AutoMapperServicesInstaller();
				Container = new WindsorContainer();
			};

		Because of = () => Installer.Install(Container, null);

		It should_resolve_the_configuration_as_a_singleton = () =>
			{
				var left = Container.Resolve<IConfiguration>();
				var right = Container.Resolve<IConfiguration>();

				left.ShouldBeTheSameAs(right);
			};

		It should_resolve_the_configuration_provider_and_the_configuration_as_a_singleton = () =>
			{
				var left = Container.Resolve<IConfigurationProvider>();
				var right = Container.Resolve<IConfiguration>();

				left.ShouldBeTheSameAs(right);
			};

		It should_resolve_the_configuration_implementation_and_the_configuration_as_a_singleton = () =>
			{
				var left = Container.Resolve<AutoMapper.Configuration>();
				var right = Container.Resolve<IConfiguration>();

				left.ShouldBeTheSameAs(right);
			};

		It should_resolve_the_mapping_engine_as_a_singleton = () =>
			{
				var left = Container.Resolve<IMappingEngine>();
				var right = Container.Resolve<IMappingEngine>();

				left.ShouldBeTheSameAs(right);
			};
	}
}