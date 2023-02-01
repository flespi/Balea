using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Balea.Extensions.AspNetCore.Hosting.Filters;

public class ApiKeyFilter : IEndpointFilter
{
	private const string API_KEY_HEADER_NAME = "X-API-Key";

	public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		var submittedApiKey = GetSubmittedApiKey(context.HttpContext);

		var apiKey = GetApiKey(context.HttpContext);

		if (!string.Equals(apiKey, submittedApiKey, StringComparison.OrdinalIgnoreCase))
		{
			return Results.Unauthorized();
		}

		return await next(context);
	}

	private static string GetSubmittedApiKey(HttpContext context)
	{
		return context.Request.Headers[API_KEY_HEADER_NAME];
	}

	private static string GetApiKey(HttpContext context)
	{
		var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

		return configuration.GetValue<string>($"ApiKey");
	}
}