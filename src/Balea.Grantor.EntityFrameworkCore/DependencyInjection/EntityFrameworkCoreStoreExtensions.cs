using Balea.Abstractions;
using Balea.Grantor.EntityFrameworkCore;
using Balea.Grantor.EntityFrameworkCore.DbContexts;
using Balea.Grantor.EntityFrameworkCore.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkCoreStoreExtensions
    {
        public static IBaleaBuilder AddEntityFrameworkCoreStore(this IBaleaBuilder builder, Action<StoreOptions> configurer = null)
        {
            var options = new StoreOptions();
            configurer?.Invoke(options);

            if (options.ConfigureDbContext != null)
            {
                builder.Services.AddDbContextPool<BaleaDbContext>(optionsAction => options.ConfigureDbContext?.Invoke(optionsAction));
            }

            builder.Services.AddScoped<IAuthorizationGrantor, EntityFrameworkAuthorizationGrantor<BaleaDbContext>>();

            return builder;
        }

        public static IBaleaBuilder AddEntityFrameworkCoreStore<TContext>(this IBaleaBuilder builder, Action<StoreOptions> configurer = null) 
            where TContext : BaleaDbContext
        {
            var options = new StoreOptions();
            configurer?.Invoke(options);

            if (options.ConfigureDbContext != null)
            {
                builder.Services.AddDbContextPool<TContext>(optionsAction => options.ConfigureDbContext?.Invoke(optionsAction));
            }

            builder.Services.AddScoped<IAuthorizationGrantor, EntityFrameworkAuthorizationGrantor<TContext>>();

            return builder;
        }
    }
}
