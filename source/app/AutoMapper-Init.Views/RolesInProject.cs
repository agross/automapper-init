using System.Collections.Generic;

namespace AutoMapper_Init.Views
{
	public class RolesInProject
	{
		public string Name
		{
			get;
			set;
		}

		public IEnumerable<string> Roles
		{
			get;
			set;
		}
	}
}