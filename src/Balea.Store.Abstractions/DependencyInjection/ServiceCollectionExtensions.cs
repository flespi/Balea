using Balea.Abstractions;
using Balea.Store;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IBaleaBuilder AddStoreGrantor(this IBaleaBuilder builder)
		{
			builder.Services.AddScoped<StoreAuthorizationGrantorFactory>();
			builder.Services.AddScoped(sp => sp.GetRequiredService<StoreAuthorizationGrantorFactory>().Create());

			return builder;
		}

		public static BaleaStoreBuilder AddBaleaStore(this IServiceCollection services)
		{
			var options = new BaleaStoreOptions();

			services.AddSingleton(options);

			return new BaleaStoreBuilder(options, services);
		}
	}
}
