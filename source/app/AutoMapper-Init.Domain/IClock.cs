using System;

namespace AutoMapper_Init.Infrastructure
{
	public interface IClock
	{
		DateTime Now
		{
			get;
		}
	}
}