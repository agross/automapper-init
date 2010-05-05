using System;

using AutoMapper;

using AutoMapper_Init.Infrastructure;

namespace AutoMapper_Init.Views.Mapping.Resolver
{
	public class CurrentDateTimeResolver:ValueResolver<object, DateTime>
	{
		readonly IClock _clock;

		public CurrentDateTimeResolver(IClock clock)
		{
			_clock = clock;
		}

		protected override DateTime ResolveCore(object source)
		{
			return _clock.Now;
		}
	}
}