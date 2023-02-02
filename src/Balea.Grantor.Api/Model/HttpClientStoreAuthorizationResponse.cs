using Balea.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Balea.Grantor.Api.Model
{
    public class HttpClientStoreAuthorizationResponse
    {
        public HttpClientStoreAuthorizationResponse()
        {
        }

        public IEnumerable<RoleResponse> Roles { get; set; }
        public DelegationResponse Delegation { get; set; }

        public AuthorizationContext To()
        {
            return new AuthorizationContext
            {
                Roles = Roles.Select(role => new Role
                {
                    Name = role.Name,
                    Description = role.Description,
                    Subjects = role.Subjects,
                    Mappings = role.Mappings,
                    Permissions = role.Permissions,
                    Enabled = role.Enabled,
                }),
                Delegation = Delegation is null
                    ? null
                    : new Delegation
                    {
                        Who = Delegation.Who,
                        Whom = Delegation.Whom,
                        From = Delegation.From,
                        To = Delegation.To,
                    },
            };
        }
    }

    public class RoleResponse
    {
        public RoleResponse()
        {

        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public IList<string> Permissions { get; set; }
        public IList<string> Mappings { get; set; }
        public IList<string> Subjects { get; set; }
    }

    public class DelegationResponse
    {
        public DelegationResponse()
        {

        }

        public string Who { get; set; }
        public string Whom { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
