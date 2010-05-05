using System;

namespace AutoMapper_Init.Domain
{
	public class Project
	{
		public string Name
		{
			get;
			set;
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