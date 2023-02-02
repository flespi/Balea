using Balea.Abstractions;
using Balea.Grantor.Configuration;
using Balea.Grantor.Configuration.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ConfigurationStoreExtensions
	{
		private const string DefaultSectionName = "Balea";

		public static IBaleaBuilder AddConfigurationStore(this IBaleaBuilder builder, IConfiguration configuration, string key = DefaultSectionName)
		{
			builder.Services.AddOptions();
			builder.Services.Configure<BaleaConfiguration>(configuration.GetSection(key));
			builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<BaleaConfiguration>>().Value);
			builder.Services.AddScoped<IAuthorizationGrantor, ConfigurationAuthorizationGrantor>();

			return builder;
		}
	}
}
