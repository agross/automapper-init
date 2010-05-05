using System;
using System.Collections.Generic;
using System.Linq;

using Castle.Windsor;

namespace AutoMapper_Init.Infrastructure
{
	public class Bootstrapper : IDisposable
	{
		public Func<IEnumerable<IWindsorInstaller>> Installers = () => new IWindsorInstaller[] { };

		public IWindsorContainer Container
		{
			get;
			private set;
		}

		/// <summary>
		///	Cleans up managed and unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			// Prevent the destructor from being called.
			GC.SuppressFinalize(this);
		}

		public Bootstrapper BootstrapApplication()
		{
			Container = new WindsorContainer().Install(Installers().ToArray());
			return this;
		}

		public Bootstrapper RunStartupConfiguration()
		{
			Container
				.ResolveAll<IRequireConfigurationOnStartup>()
				.Each(x => x.Configure());

			return this;
		}

		/// <summary>
		///	Central method for cleaning up resources.
		/// </summary>
		protected virtual void Dispose(bool @explicit)
		{
			// If explicit is true, then this method was called through
			// the public Dispose().
			if (@explicit)
			{
				// Release or cleanup managed resources.
				if (Container != null)
				{
					Container.Dispose();
					Container = null;
				}
			}

			// Always release or cleanup (any) unmanaged resources.
		}

		/// <summary>
		///	Finalizes an instance of the <see cref="Bootstrapper" /> class.
		/// </summary>
		/// <remarks>
		///	This destructor will run only if the Dispose method does not get called.
		///	The destructor is called indeterministicly by the Garbage Collector.
		/// </remarks>
		~Bootstrapper()
		{
			// Since other managed ob jects are disposed automatically,
			// we should not try to dispose any managed resources.
			// We therefore pass false to Dispose().
			Dispose(false);
		}
	}
}