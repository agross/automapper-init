using AutoMapper_Init.Application;
using AutoMapper_Init.Infrastructure;
using AutoMapper_Init.Infrastructure.AutoMapperIntegration;

using Castle.Windsor;

namespace AutoMapper_Init.Console
{
	internal class Program
	{
		static void Main(string[] args)
		{
			using(var bootstrapper = new Bootstrapper())
			{
				bootstrapper.Installers = () =>
					{
						return new IWindsorInstaller[]
						       {
						       	new AutoMapperServicesInstaller(),
						       	new ContainerConfiguration()
						       };
					};

				bootstrapper
					.BootstrapApplication()
					.RunStartupConfiguration();

				var app = bootstrapper.Container.Resolve<IEntryPoint>();
				app.Run();
			}
		}
	}
}