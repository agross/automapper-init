using System;
using System.Globalization;

using AutoMapper;

using AutoMapper_Init.Domain;
using AutoMapper_Init.Infrastructure;
using AutoMapper_Init.Views.Mapping;
using AutoMapper_Init.Views.Mapping.Formatter;
using AutoMapper_Init.Views.Mapping.Resolver;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Machine.Specifications;

using Rhino.Mocks;

namespace AutoMapper_Init.Views.Tests.Mapping
{
	[Subject(typeof(ProjectToProjectSummary))]
	public class When_a_project_is_mapped_to_the_project_summary
	{
		static Project Project;
		static ProjectSummary View;

		Establish context = () =>
			{
				Mapper.Initialize(x =>
					{
						var clock = MockRepository.GenerateStub<IClock>();
						clock
							.Stub(y => y.Now)
							.Return(new DateTime(2010, 5, 4, 3, 2, 1));

						var container = new WindsorContainer()
							.Register(Component.For<IClock>().Instance(clock),
							          Component.For<CurrentDateTimeResolver>().ImplementedBy<CurrentDateTimeResolver>(),
							          Component.For<EnglishDateFormatter>().ImplementedBy<EnglishDateFormatter>());
						
						x.ConstructServicesUsing(container.Resolve);

						new ProjectToProjectSummary().RegisterMapping(x);
					});

				Project = new Project("Apollo")
				          {
				          	StartedAt = DateTime.MaxValue,
				          	ProjectLead = new User("Peter")
				          	              {
				          	              	Roles = new[] { new Role("User") }
				          	              }
				          };
			};

		Because of = () => { View = Mapper.Map<Project, ProjectSummary>(Project); };

		It should_map_the_project_name =
			() => View.Name.ShouldEqual("Apollo");

		It should_map_the_project_lead_s_name =
			() => View.ProjectLeadName.ShouldEqual("Peter");

		It should_map_the_project_s_start_date_as_an_English_date =
			() => View.ProjectStartDate.ShouldEqual(DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture));

		It should_map_the_current_date_and_time =
			() => View.Now.ShouldNotEqual(DateTime.MinValue);
	}
}