using Balea.Api.Store.Model;
using Balea.Model;

namespace Balea.Extensions.AspNetCore.Hosting;

internal static partial class Map
{
    public static HttpClientStorePolicyResponse From(Policy source)
        => new()
        {
            Name = source.Name,
            Content = source.Content,
        };
}
