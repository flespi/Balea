using System.Collections.Generic;

using Balea.Model;

namespace Balea.Grantor.Configuration;

public class Application
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public IList<Role> Roles { get; set; }
    public IList<Delegation> Delegations { get; set; }
    public IList<Policy> Policies { get; set; }
}