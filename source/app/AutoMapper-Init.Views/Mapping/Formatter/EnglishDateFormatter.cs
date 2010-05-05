using System;
using System.Globalization;

using AutoMapper;

namespace AutoMapper_Init.Views.Mapping.Formatter
{
	public class EnglishDateFormatter : ValueFormatter<DateTime>
	{
		protected override string FormatValueCore(DateTime value)
		{
			return value.Date.ToString(CultureInfo.InvariantCulture);
		}
	}
}