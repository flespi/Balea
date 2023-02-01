using Asp.Versioning.Conventions;
using Balea;
using Balea.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Balea.Api.Store.Model;
using Balea.Extensions.AspNetCore.Hosting;
using Balea.Extensions.AspNetCore.Hosting.Filters;

using static Balea.Extensions.AspNetCore.Hosting.Utils;
using static Balea.Extensions.AspNetCore.Hosting.Versions;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationExtensions
{
	public static WebApplication UseBaleaEndpoints(this WebApplication app)
	{
		var versions = app.NewApiVersionSet()
			.HasApiVersion(1)
			.Build();

		app.MapGet("/api/users/{user}/applications/{application}/authorization",
			async (
				[FromHeader(Name = "X-API-Key")] string apiKey,
				[FromRoute] string user,
				[FromRoute] string application,
				[FromQuery] string[]? roles,
				IRuntimeAuthorizationServerStore store,
				BaleaOptions options
				) =>
			{
				var principal = CreatePrincipal(options, user, roles);
				var context = await store.FindAuthorizationAsync(principal);

				if (context is null)
				{
					return Results.NotFound();
				}

				var response = Map.From(context);

				return Results.Ok(response);
			})
			.Produces<HttpClientStoreAuthorizationResponse>()
			.AddEndpointFilter<ApiKeyFilter>()
			.WithApiVersionSet(versions)
			.MapToApiVersion(VERSION_1_0);

		app.MapGet("/api/applications/{application}/policies/{policy}",
			async (
				[FromHeader(Name = "X-API-Key")] string apiKey,
				[FromRoute] string application,
				[FromRoute] string policy,
				IRuntimeAuthorizationServerStore store
				) =>
			{
				var result = await store.GetPolicyAsync(policy);

				if (result is null)
				{
					return Results.NotFound();
				}

				var response = Map.From(result);

				return Results.Ok(response);
			})
			.Produces<HttpClientStorePolicyResponse>()
			.AddEndpointFilter<ApiKeyFilter>()
			.WithApiVersionSet(versions)
			.MapToApiVersion(VERSION_1_0);

		return app;
	}
}
