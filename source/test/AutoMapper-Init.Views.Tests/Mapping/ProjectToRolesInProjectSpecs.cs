using System;

using AutoMapper;

using AutoMapper_Init.Domain;
using AutoMapper_Init.Views.Mapping;

using Machine.Specifications;

namespace AutoMapper_Init.Views.Tests.Mapping
{
	[Subject(typeof(ProjectToRolesInProject))]
	public class When_a_project_is_mapped_to_roles_in_the_project
	{
		static Project Project;
		static RolesInProject View;

		Establish context = () =>
			{
				Mapper.Initialize(x =>
					{
						new UserToRoles().RegisterMapping(x);
						new ProjectToRolesInProject().RegisterMapping(x);
					});

				Project = new Project("Apollo")
				          {
				          	StartedAt = DateTime.MaxValue,
				          	ProjectLead = new User("Peter")
				          	              {
				          	              	Roles = new[] { new Role("User"), new Role("Administrator") }
				          	              }
				          };
			};

		Because of = () => { View = Mapper.Map<Project, RolesInProject>(Project); };

		It should_map_the_project_name =
			() => View.Name.ShouldEqual("Apollo");

		It should_map_the_project_lead_s_roles =
			() => View.Roles.ShouldContainOnly(new[]{"Administrator", "User"});
		

	}
}