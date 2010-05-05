using AutoMapper;

namespace AutoMapper_Init.Infrastructure.AutoMapperIntegration
{
	public interface IMapper
	{
		void RegisterMapping(IConfiguration configuration);
	}
}