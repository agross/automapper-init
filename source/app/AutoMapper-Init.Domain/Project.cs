using System;

namespace AutoMapper_Init.Domain
{
	public class Project
	{
		public Project(string name)
		{
			Name = name;
		}

		public string Name
		{
			get;
			private set;
		}

		public DateTime StartedAt
		{
			get;
			set;
		}

		public User ProjectLead
		{
			get;
			set;
		}
	}
}