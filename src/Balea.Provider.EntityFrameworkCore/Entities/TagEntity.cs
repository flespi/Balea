using System.Collections.Generic;

namespace Balea.Provider.EntityFrameworkCore.Entities
{
	public class TagEntity
	{
		public TagEntity(string name)
		{
			Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<PermissionTagEntity> Permissions { get; set; } = new List<PermissionTagEntity>();
		public ICollection<RoleTagEntity> Roles { get; set; } = new List<RoleTagEntity>();
	}
}
