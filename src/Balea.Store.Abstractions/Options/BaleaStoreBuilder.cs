using System;

using Microsoft.Extensions.DependencyInjection;

namespace Balea.Store
{
	public class BaleaStoreBuilder
	{
		private BaleaStoreOptions Options { get; }

		public IServiceCollection Services { get; }

		internal BaleaStoreBuilder(BaleaStoreOptions options, IServiceCollection services)
		{
			Options = options;
			Services = services;
		}

		public BaleaStoreBuilder AddDelegations<TDelegation>()
			where TDelegation : class
		{
			Options.DelegationType = typeof(TDelegation);
			return this;
		}

		public BaleaStoreBuilder AddDelegationStore<TDelegationStore>()
			where TDelegationStore : class
		{
			if (Options.DelegationType is null)
			{
				throw new InvalidOperationException();
			}

			Services.AddScoped(typeof(IDelegationStore<>).MakeGenericType(Options.DelegationType), typeof(TDelegationStore));

			return this;
		}

		public BaleaStoreBuilder AddPolicies<TPolicy>()
			where TPolicy : class
		{
			Options.PolicyType = typeof(TPolicy);
			return this;
		}

		public BaleaStoreBuilder AddPolicyStore<TPolicyStore>()
			where TPolicyStore : class
		{
			if (Options.PolicyType is null)
			{
				throw new InvalidOperationException();
			}

			Services.AddScoped(typeof(IPolicyStore<>).MakeGenericType(Options.PolicyType), typeof(TPolicyStore));

			return this;
		}

		public BaleaStoreBuilder AddRoles<TRole>()
			where TRole : class
		{
			Options.RoleType = typeof(TRole);
			return this;
		}

		public BaleaStoreBuilder AddRoleStore<TRoleStore>()
			where TRoleStore : class
		{
			if (Options.DelegationType is null)
			{
				throw new InvalidOperationException();
			}

			Services.AddScoped(typeof(IRoleStore<>).MakeGenericType(Options.RoleType), typeof(TRoleStore));

			return this;
		}
	}
}
