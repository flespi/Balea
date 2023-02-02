using System.Collections.Generic;

namespace Balea.Model
{
	public class Role
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Enabled { get; set; }
		public IList<string> Subjects { get; set; }
		public IList<string> Mappings { get; set; }
		public IList<string> Permissions { get; set; }
	}
}
