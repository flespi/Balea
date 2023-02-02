using System.Collections.Generic;

namespace Balea.Provider.EntityFrameworkCore.Entities
{
	public class MappingEntity
	{
		public MappingEntity(string name)
		{
			Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<RoleMappingEntity> Roles { get; set; }
	}
}
