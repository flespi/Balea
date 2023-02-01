using Asp.Versioning;

using static Balea.Extensions.AspNetCore.Hosting.Versions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddBaleaEndpoints(this IServiceCollection services)
	{
		services.AddApiVersioning(options =>
		{
			options.DefaultApiVersion = VERSION_1_0;
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ApiVersionReader = new QueryStringApiVersionReader();
		});

		return services;
	}
}
