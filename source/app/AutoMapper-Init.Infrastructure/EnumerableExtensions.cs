using System;
using System.Collections.Generic;

namespace AutoMapper_Init.Infrastructure
{
	public static class EnumerableExtensions
	{
		public static void Each<T>(this IEnumerable<T> instance, Action<T> action)
		{
			foreach (var i in instance)
			{
				action(i);
			}
		}
	}
}