using System.Collections.Generic;

namespace AutoMapper_Init.Domain
{
	public class User
	{
		public User(string name)
		{
			Name = name;
		}

		public string Name
		{
			get;
			private set;
		}

		public IEnumerable<Role> Roles
		{
			get;
			set;
		}
	}
}