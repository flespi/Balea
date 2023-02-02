using System;

using Balea.Model;
using Balea.Store;
using Balea.Store;
using Balea.Store.Configuration;
using Balea.Store.Configuration.Options;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ConfigurationStoreExtensions
	{
		public static BaleaStoreBuilder UseConfiguration(this BaleaStoreBuilder builder, Action<StoreOptions> configurer = null)
		{
			var options = new StoreOptions();
			configurer?.Invoke(options);

			builder.Services.AddSingleton(options);

			builder
				.AddDelegations<Delegation>()
				.AddDelegationStore<DelegationStore>()
				.AddPolicies<Policy>()
				.AddPolicyStore<PolicyStore>()
				.AddRoles<Role>()
				.AddRoleStore<RoleStore>();

			return builder;
		}
	}
}
