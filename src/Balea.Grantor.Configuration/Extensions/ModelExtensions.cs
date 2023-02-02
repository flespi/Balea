using Balea.Model;
using System;
using System.Linq;

namespace Balea.Grantor.Configuration.Model
{
    public static class ModelExtensions
    {
        public static Role To(this RoleConfiguration role)
        {
            if (role is null)
            {
                return null;
            }

            return new Role
            {
                Name = role.Name,
                Description = role.Description,
                Subjects = role.Subjects,
                Mappings = role.Mappings,
                Permissions = role.Permissions.Distinct().ToList(),
                Enabled = role.Enabled,
            };
        }

        public static Delegation To(this DelegationConfiguration delegation)
        {
            if (delegation is null)
            {
                return null;
            }

            return new Delegation
            {
                Who = delegation.Who,
                Whom = delegation.Whom,
                From = delegation.From,
                To = delegation.To,
            };
        }

        public static DelegationConfiguration GetCurrentDelegation(this DelegationConfiguration[] delegations, string subjectId)
        {
            return delegations.FirstOrDefault(d => d.Active && d.Whom == subjectId);
        }

        public static ApplicationConfiguration GetByName(this ApplicationConfiguration[] applications, string name)
        {
            return applications.First(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
