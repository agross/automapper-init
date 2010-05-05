using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using AutoMapper_Init.Domain;
using AutoMapper_Init.Infrastructure.AutoMapperIntegration;

namespace AutoMapper_Init.Views.Mapping
{
	public class UserToRoles : IMapper
	{
		public void RegisterMapping(IConfiguration configuration)
		{
			configuration.CreateMap<User, IEnumerable<string>>()
				.ConvertUsing(x => x.Roles.Select(y => y.Name));
		}
	}
}