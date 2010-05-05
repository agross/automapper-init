using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using AutoMapper.Mappers;

using Castle.Facilities.FactorySupport;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace AutoMapper_Init.Infrastructure.AutoMapperIntegration
{
	public class AutoMapperServicesInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			if (!container.Kernel.GetFacilities().Any(x => x is FactorySupportFacility))
			{
				container.AddFacility<FactorySupportFacility>();
			}

			container.Register(DefaultRegistrations().ToArray());
		}

		static IEnumerable<IRegistration> DefaultRegistrations()
		{
			yield return Component.For<IMappingEngine>()
				.LifeStyle.Singleton
				.ImplementedBy<MappingEngine>();

			yield return Component.For<Configuration>()
				.LifeStyle.Singleton
				.UsingFactoryMethod(() => new Configuration(new TypeMapFactory(), MapperRegistry.AllMappers()))
				.Forward<IConfigurationProvider>()
				.Forward<IConfiguration>();
		}
	}
}