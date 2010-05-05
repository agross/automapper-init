using System;

using AutoMapper;

using AutoMapper_Init.Domain;
using AutoMapper_Init.Infrastructure;
using AutoMapper_Init.Views;

namespace AutoMapper_Init.Application
{
	public class MainApplication : IEntryPoint
	{
		readonly IMappingEngine _mappingEngine;

		public MainApplication(IMappingEngine mappingEngine)
		{
			_mappingEngine = mappingEngine;
		}

		public void Run()
		{
			var project = CreateSampleData();

			var summary = _mappingEngine.Map<Project, ProjectSummary>(project);
			Output(summary);

			var roles = _mappingEngine.Map<Project, RolesInProject>(project);
			Output(roles);

			Console.WriteLine("Done");
			Console.ReadLine();
		}

		static Project CreateSampleData()
		{
			return new Project("Apollo")
			       {
			       	StartedAt = new DateTime(2007, 3, 4, 12, 45, 33),
			       	ProjectLead = new User("Peter")
			       	              {
			       	              	Roles = new[] { new Role("User"), new Role("Contributor"), new Role("Member") }
			       	              }
			       };
		}

		static void Output(ProjectSummary summary)
		{
			Console.WriteLine("Summary of project {0} started at {1} led by {2} as of {3}",
			                  summary.Name,
			                  summary.ProjectStartDate,
			                  summary.ProjectLeadName,
			                  summary.Now);
		}

		static void Output(RolesInProject roles)
		{
			Console.WriteLine("Roles in project {0}:", roles.Name);
			roles.Roles.Each(x => Console.WriteLine("Roles {0}", x));
		}
	}
}