using Balea.Api.Store.Model;
using Balea.Model;

namespace Balea.Extensions.AspNetCore.Hosting;

internal static partial class Map
{
    public static RoleResponse From(Role source)
        => new()
        {
            Name = source.Name,
            Description = source.Description,
            Subjects = source.GetSubjects(),
            Mappings = source.GetMappings(),
            Permissions = source.GetPermissions(),
            Enabled = source.Enabled,
        };
}
