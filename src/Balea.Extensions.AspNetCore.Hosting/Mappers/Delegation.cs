using Balea.Api.Store.Model;
using Balea.Model;

namespace Balea.Extensions.AspNetCore.Hosting;

internal static partial class Map
	{
    public static DelegationResponse From(Delegation source)
        => new()
        {
            Who = source.Who,
            Whom = source.Whom,
            From = source.From,
            To = source.To,
        };
}
