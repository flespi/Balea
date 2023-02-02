using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Balea.Abstractions;
using Balea.Model;

namespace Balea.Store
{
	public class StoreAuthorizationGrantor<TDelegation, TPolicy, TRole> : IAuthorizationGrantor
		where TDelegation : class
		where TPolicy : class
		where TRole : class
	{
		private readonly BaleaOptions _options;

		private readonly IDelegationStore<TDelegation> _delegationStore;
		private readonly IPolicyStore<TPolicy> _policyStore;
		private readonly IRoleStore<TRole> _roleStore;

		public StoreAuthorizationGrantor(
			BaleaOptions options,
			IDelegationStore<TDelegation> delegationStore,
			IPolicyStore<TPolicy> policyStore,
			IRoleStore<TRole> roleStore
			)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));

			_delegationStore = delegationStore ?? throw new ArgumentNullException(nameof(delegationStore));
			_policyStore = policyStore ?? throw new ArgumentNullException(nameof(policyStore));
			_roleStore = roleStore ?? throw new ArgumentNullException(nameof(roleStore));
		}

		public async Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
		{
			var sourceRoleClaims = user.GetClaimValues(_options.ClaimTypeMap.RoleClaimType).ToList();

			var subject = user.GetSubjectId(_options);

			var delegation = await _delegationStore.FindBySubjectAsync(subject, cancellationToken);

			if (delegation != null)
			{
				subject = await _delegationStore.GetWhoAsync(delegation, cancellationToken);
			}

			var subjectRoles = await _roleStore.FindByMemberAsync(subject, cancellationToken);
			var mappingRoles = await _roleStore.FindByMappingsAsync(sourceRoleClaims, cancellationToken);

			var roles = subjectRoles.Union(mappingRoles).Distinct();

			var contextRoles = new List<Role>();

			foreach (var role in roles)
			{
				contextRoles.Add(await GetRoleInfoAsync(role, cancellationToken));
			}

			var contextDelegation = delegation is null ? null : await GetDelegationInfoAsync(delegation, cancellationToken);

			return new AuthorizationContext(contextRoles, contextDelegation);
		}

		public async Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default)
		{
			var policy = await _policyStore.FindByNameAsync(name, cancellationToken);
			var content = await _policyStore.GetContentAsync(policy, cancellationToken);

			return new Policy(name, content);
		}

		private async Task<Role> GetRoleInfoAsync(TRole role, CancellationToken cancellationToken)
		{
			var name = await _roleStore.GetNameAsync(role, cancellationToken);
			var description = await _roleStore.GetDescriptionAsync(role, cancellationToken);
			var enabled = await _roleStore.IsEnabledAsync(role, cancellationToken);

			var subjects = await _roleStore.GetMembersAsync(role, cancellationToken);
			var mappings = await _roleStore.GetMappingsAsync(role, cancellationToken);
			var permissions = await _roleStore.GetPermissionsAsync(role, cancellationToken);

			return new Role(name, description, subjects, mappings, permissions, enabled);
		}

		private async Task<Delegation> GetDelegationInfoAsync(TDelegation delegation, CancellationToken cancellationToken)
		{
			var who = await _delegationStore.GetWhoAsync(delegation, cancellationToken);
			var whom = await _delegationStore.GetWhomAsync(delegation, cancellationToken);
			var from = await _delegationStore.GetFromAsync(delegation, cancellationToken);
			var to = await _delegationStore.GetToAsync(delegation, cancellationToken);

			return new Delegation(who, whom, from, to);
		}
	}
}
