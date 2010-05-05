using System.Collections.Generic;
using System.Linq;

using AutoMapper_Init.Infrastructure;
using AutoMapper_Init.Infrastructure.AutoMapperIntegration;
using AutoMapper_Init.Views;

using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;

namespace AutoMapper_Init.Application
{
	public class ContainerConfiguration : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

			container.Register(CreateRegistrations(container).ToArray());
		}

		IEnumerable<IRegistration> CreateRegistrations(IWindsorContainer container)
		{
			yield return Component
				.For<IWindsorContainer>()
				.Instance(container);
			
			yield return Component
				.For<IEntryPoint>()
				.ImplementedBy<MainApplication>();

			yield return Component
				.For<IClock>()
				.ImplementedBy<SystemClock>();

			yield return Component
				.For<IRequireConfigurationOnStartup>()
				.ImplementedBy<AutoMapperInitializer>();

			// All IMappers.
			yield return AllTypes
				.FromAssembly(typeof(ProjectSummary).Assembly)
				.BasedOn<IMapper>()
				.WithService.FirstInterface();

			// All mapping support types.
			yield return AllTypes
				.FromAssembly(typeof(ProjectSummary).Assembly)
				.Where(x => x.Namespace.Contains("Mapping"))
				.Where(x => x.Namespace.Contains("Formatter"))
				.Where(x => x.Namespace.Contains("Resolver"));
		}
	}
}