using AutoMapper;

using AutoMapper_Init.Domain;
using AutoMapper_Init.Infrastructure.AutoMapperIntegration;

namespace AutoMapper_Init.Views.Mapping
{
	public class ProjectToRolesInProject : IMapper
	{
		public void RegisterMapping(IConfiguration configuration)
		{
			configuration.CreateMap<Project, RolesInProject>()
				.ForMember(x => x.Roles, x => x.MapFrom(y => y.ProjectLead));
		}
	}
}