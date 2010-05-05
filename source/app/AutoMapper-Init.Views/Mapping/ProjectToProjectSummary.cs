using AutoMapper;

using AutoMapper_Init.Domain;
using AutoMapper_Init.Infrastructure.AutoMapperIntegration;
using AutoMapper_Init.Views.Mapping.Formatter;
using AutoMapper_Init.Views.Mapping.Resolver;

namespace AutoMapper_Init.Views.Mapping
{
	public class ProjectToProjectSummary : IMapper
	{
		public void RegisterMapping(IConfiguration configuration)
		{
			configuration.CreateMap<Project, ProjectSummary>()
				.ForMember(x => x.ProjectStartDate,
				           x =>
				           	{
				           		x.MapFrom(y => y.StartedAt);
				           		x.AddFormatter<EnglishDateFormatter>();
				           	})
				.ForMember(x => x.Now, x => x.ResolveUsing<CurrentDateTimeResolver>());
		}
	}
}