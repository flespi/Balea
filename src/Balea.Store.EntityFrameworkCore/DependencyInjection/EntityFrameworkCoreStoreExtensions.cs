using System;

using Balea.Provider.EntityFrameworkCore.DbContexts;
using Balea.Provider.EntityFrameworkCore.Entities;

using Balea.Store;
using Balea.Store.EntityFrameworkCore;
using Balea.Store.EntityFrameworkCore.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class EntityFrameworkCoreStoreExtensions
{
    public static BaleaStoreBuilder UseEntityFrameworkCore(this BaleaStoreBuilder builder, Action<StoreOptions> configurer = null)
    {
        return UseEntityFrameworkCore<BaleaDbContext>(builder, configurer);
    }

    public static BaleaStoreBuilder UseEntityFrameworkCore<TContext>(this BaleaStoreBuilder builder, Action<StoreOptions> configurer = null)
        where TContext : BaleaDbContext
    {
        var options = new StoreOptions();
        configurer?.Invoke(options);

        builder
            .AddDelegations<DelegationEntity>()
            .AddDelegationStore<DelegationStore>()
            .AddPolicies<PolicyEntity>()
            .AddPolicyStore<PolicyStore>()
            .AddRoles<RoleEntity>()
            .AddRoleStore<RoleStore>();

        if (options.ConfigureDbContext != null)
        {
            builder.Services.AddDbContextPool<TContext>(optionsAction => options.ConfigureDbContext?.Invoke(optionsAction));
        }

        return builder;
    }
}