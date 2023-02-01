using System.Linq;

using Balea.Api.Store.Model;
using Balea.Model;

namespace Balea.Extensions.AspNetCore.Hosting;

internal static partial class Map
{
    public static HttpClientStoreAuthorizationResponse From(AuthorizationContext source)
        => new()
        {
            Roles = source.Roles.Select(role => From(role)),
            Delegation = source.Delegation is null ? null : From(source.Delegation),
        };
}
