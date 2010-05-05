using AutoMapper;

using AutoMapper_Init.Infrastructure.AutoMapperIntegration;

using Castle.Windsor;

using Machine.Specifications;
using Machine.Specifications.Utility;

using Rhino.Mocks;

namespace AutoMapper_Init.Infrastructure.Tests.AutoMapperIntegration
{
	[Subject(typeof(AutoMapperInitializer))]
	public class When_auto_mapper_is_initialized
	{
		static AutoMapperInitializer Initializer;
		static IConfigurationProvider ConfigurationProvider;
		static IMapper[] Mappers;
		static IConfiguration Configuration;

		Establish context = () =>
		{
			ConfigurationProvider = MockRepository.GenerateStub<IConfigurationProvider>();
			Configuration = MockRepository.GenerateStub<IConfiguration>();

			Mappers = new[]
				          {
				          	MockRepository.GenerateStub<IMapper>(),
				          	MockRepository.GenerateStub<IMapper>()
				          };

			Initializer = new AutoMapperInitializer(ConfigurationProvider,
													Configuration,
													MockRepository.GenerateStub<IWindsorContainer>())
			{
				Mappers = Mappers
			};
		};

		Because of = () => Initializer.Configure();

		It should_register_all_mappers =
			() => RandomExtensionMethods.Each(Mappers, x => x.AssertWasCalled(y => y.RegisterMapping(Configuration)));

		It should_ensure_that_the_configuration_is_valid =
			() => ConfigurationProvider.AssertWasCalled(x => x.AssertConfigurationIsValid());
	}
}