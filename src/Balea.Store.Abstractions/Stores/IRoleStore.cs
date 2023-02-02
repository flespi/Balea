using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Balea.Store
{
	public interface IRoleStore<TRole>
		where TRole : class
	{
		Task<TRole> FindByNameAsync(string roleName, CancellationToken cancellationToken);

		Task<AccessResult> CreateAsync(TRole role, CancellationToken cancellationToken);
		Task<AccessResult> UpdateAsync(TRole role, CancellationToken cancellationToken);
		Task<AccessResult> DeleteAsync(TRole role, CancellationToken cancellationToken);

		Task<string> GetNameAsync(TRole role, CancellationToken cancellationToken);
		Task<string> GetDescriptionAsync(TRole role, CancellationToken cancellationToken);
		Task<bool> IsEnabledAsync(TRole role, CancellationToken cancellationToken);

		Task<AccessResult> AddMappingAsync(TRole role, string mapping, CancellationToken cancellationToken);
		Task<AccessResult> AddMappingsAsync(TRole role, IList<string> mappings, CancellationToken cancellationToken);
		Task<AccessResult> RemoveMappingAsync(TRole role, string mapping, CancellationToken cancellationToken);
		Task<AccessResult> RemoveMappingsAsync(TRole role, IList<string> mappings, CancellationToken cancellationToken);
		Task<IList<string>> GetMappingsAsync(TRole role, CancellationToken cancellationToken);
		Task<IList<TRole>> FindByMappingAsync(string mapping, CancellationToken cancellationToken);
		Task<IList<TRole>> FindByMappingsAsync(IList<string> mappings, CancellationToken cancellationToken);

		Task<AccessResult> AddPermissionAsync(TRole role, string permission, CancellationToken cancellationToken);
		Task<AccessResult> AddPermissionsAsync(TRole role, IList<string> permissions, CancellationToken cancellationToken);
		Task<AccessResult> RemovePermissionAsync(TRole role, string permission, CancellationToken cancellationToken);
		Task<AccessResult> RemovePermissionsAsync(TRole role, IList<string> permissions, CancellationToken cancellationToken);
		Task<IList<string>> GetPermissionsAsync(TRole role, CancellationToken cancellationToken);
		Task<IList<TRole>> FindByPermissionAsync(string permission, CancellationToken cancellationToken);

		Task<AccessResult> AddMemberAsync(TRole role, string subject, CancellationToken cancellationToken);
		Task<AccessResult> AddMembersAsync(TRole role, IList<string> subjects, CancellationToken cancellationToken);
		Task<AccessResult> RemoveMemberAsync(TRole role, string subject, CancellationToken cancellationToken);
		Task<AccessResult> RemoveMembersAsync(TRole role, IList<string> subjects, CancellationToken cancellationToken);
		Task<IList<string>> GetMembersAsync(TRole role, CancellationToken cancellationToken);
		Task<IList<TRole>> FindByMemberAsync(string subject, CancellationToken cancellationToken);
	}
}
