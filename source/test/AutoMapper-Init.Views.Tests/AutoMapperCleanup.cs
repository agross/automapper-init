using AutoMapper;

using Machine.Specifications;

namespace AutoMapper_Init.Views.Tests
{
	public class AutoMapperCleanup : ICleanupAfterEveryContextInAssembly
	{
		public void AfterContextCleanup()
		{
			Mapper.Reset();
		}
	}
}