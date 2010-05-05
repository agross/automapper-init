using System;

namespace AutoMapper_Init.Infrastructure
{
	public class SystemClock : IClock
	{
		public DateTime Now
		{
			get { return DateTime.Now; }
		}
	}
}