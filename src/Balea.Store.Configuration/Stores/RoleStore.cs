using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Balea.Abstractions;
using Balea.Model;
using Balea.Provider.Configuration.Model;
using Balea.Store.Configuration.Options;

namespace Balea.Store.Configuration
{
	public class RoleStore : IRoleStore<Role>
	{
		private readonly StoreOptions _options;
		private readonly IAppContextAccessor _contextAccessor;

		public RoleStore(
			StoreOptions options,
			IAppContextAccessor contextAccessor)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
		}

		public Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			var policy = application.Roles.FirstOrDefault(x => x.Name == roleName);

			return Task.FromResult(policy);
		}

		public Task<AccessResult> CreateAsync(Role role, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			application.Roles.Add(role);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> UpdateAsync(Role role, CancellationToken cancellationToken)
		{
			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> DeleteAsync(Role role, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			application.Roles.Remove(role);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<string> GetNameAsync(Role role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.Name);
		}

		public Task<string> GetDescriptionAsync(Role role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.Description);
		}

		public Task<bool> IsEnabledAsync(Role role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.Enabled);
		}

		public Task<AccessResult> AddMappingAsync(Role role, string mapping, CancellationToken cancellationToken)
		{
			role.Mappings.Add(mapping);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> AddMappingsAsync(Role role, IList<string> mappings, CancellationToken cancellationToken)
		{
			foreach (var mapping in mappings)
			{
				role.Mappings.Add(mapping);
			}

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> RemoveMappingAsync(Role role, string mapping, CancellationToken cancellationToken)
		{
			role.Mappings.Remove(mapping);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> RemoveMappingsAsync(Role role, IList<string> mappings, CancellationToken cancellationToken)
		{
			foreach (var mapping in mappings)
			{
				role.Mappings.Remove(mapping);
			}

			return Task.FromResult(AccessResult.Success);
		}

		public Task<IList<string>> GetMappingsAsync(Role role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.Mappings);
		}

		public Task<IList<Role>> FindByMappingAsync(string mapping, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			var roles = application.Roles.Where(x => x.Mappings.Contains(mapping)).ToList();

			return Task.FromResult<IList<Role>>(roles);
		}

		public Task<IList<Role>> FindByMappingsAsync(IList<string> mappings, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			var roles = application.Roles.Where(x => mappings.Any(mapping => x.Mappings.Contains(mapping))).ToList();

			return Task.FromResult<IList<Role>>(roles);
		}

		public Task<AccessResult> AddPermissionAsync(Role role, string permission, CancellationToken cancellationToken)
		{
			role.Permissions.Add(permission);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> AddPermissionsAsync(Role role, IList<string> permissions, CancellationToken cancellationToken)
		{
			foreach (var permission in permissions)
			{
				role.Permissions.Add(permission);
			}

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> RemovePermissionAsync(Role role, string permission, CancellationToken cancellationToken)
		{
			role.Permissions.Remove(permission);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> RemovePermissionsAsync(Role role, IList<string> permissions, CancellationToken cancellationToken)
		{
			foreach (var permission in permissions)
			{
				role.Permissions.Remove(permission);
			}

			return Task.FromResult(AccessResult.Success);
		}

		public Task<IList<string>> GetPermissionsAsync(Role role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.Mappings);
		}

		public Task<IList<Role>> FindByPermissionAsync(string permission, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			var roles = application.Roles.Where(x => x.Permissions.Contains(permission)).ToList();

			return Task.FromResult<IList<Role>>(roles);
		}

		public Task<AccessResult> AddMemberAsync(Role role, string subject, CancellationToken cancellationToken)
		{
			role.Subjects.Add(subject);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> AddMembersAsync(Role role, IList<string> subjects, CancellationToken cancellationToken)
		{
			foreach (var subject in subjects)
			{
				role.Subjects.Add(subject);
			}

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> RemoveMemberAsync(Role role, string subject, CancellationToken cancellationToken)
		{
			role.Subjects.Remove(subject);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> RemoveMembersAsync(Role role, IList<string> subjects, CancellationToken cancellationToken)
		{
			foreach (var subject in subjects)
			{
				role.Subjects.Remove(subject);
			}

			return Task.FromResult(AccessResult.Success);
		}

		public Task<IList<string>> GetMembersAsync(Role role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.Subjects);
		}

		public Task<IList<Role>> FindByMemberAsync(string subject, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			var roles = application.Roles.Where(x => x.Subjects.Contains(subject)).ToList();

			return Task.FromResult<IList<Role>>(roles);
		}
	}
}
