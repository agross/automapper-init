using System.Collections.Generic;

namespace AutoMapper_Init.Domain
{
	public class User
	{
		public string Name
		{
			get;
			set;
		}

		public IEnumerable<Role> Roles
		{
			get;
			set;
		}
	}
}