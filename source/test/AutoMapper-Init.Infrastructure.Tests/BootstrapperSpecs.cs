using Castle.MicroKernel.Registration;

using Machine.Specifications;

using Rhino.Mocks;

namespace AutoMapper_Init.Infrastructure.Tests
{
	[Subject(typeof(Bootstrapper))]
	public class When_the_application_starts_up
	{
		static Bootstrapper Bootstrapper;
		static IRequireConfigurationOnStartup[] RequiresConfig;

		Establish context = () =>
			{
				Bootstrapper = new Bootstrapper().BootstrapApplication();

				RequiresConfig = new[]
				                 {
				                 	MockRepository.GenerateStub<IRequireConfigurationOnStartup>(),
				                 	MockRepository.GenerateStub<IRequireConfigurationOnStartup>()
				                 };

				Bootstrapper.Container.Register(Component
				                                	.For<IRequireConfigurationOnStartup>()
				                                	.Named("one")
				                                	.Instance(RequiresConfig[0]),
				                                Component
				                                	.For<IRequireConfigurationOnStartup>()
				                                	.Named("two")
				                                	.Instance(RequiresConfig[1]));
			};

		Because of = () => Bootstrapper.RunStartupConfiguration();

		It should_configure_all_components_that_require_configuration =
			() => RequiresConfig.Each(x => x.AssertWasCalled(y => y.Configure()));
	}
}