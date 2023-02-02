using System.Collections.Generic;

namespace Balea.Model
{
	public class AuthorizationContext
	{
		public IEnumerable<Role> Roles { get; set; }

		public Delegation Delegation { get; set; }
	}
}
