using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Balea.Model;

using Microsoft.EntityFrameworkCore;

namespace Balea.Provider.EntityFrameworkCore.Entities;

public static class EntitiesExtensions
{
    public static Role To(this RoleEntity role)
    {
        if (role is null)
        {
            return null;
        }

        return new Role
        {
            Name = role.Name,
            Description = role.Description,
            Subjects = role.Subjects.Select(rs => rs.Subject.Sub).ToList(),
            Mappings = role.Mappings.Select(rm => rm.Mapping.Name).ToList(),
            Permissions = role.Permissions.Select(rp => rp.Permission.Name).ToList(),
            Enabled = role.Enabled,
        };
    }

    public static Delegation To(this DelegationEntity delegation)
    {
        if (delegation is null)
        {
            return null;
        }

        return new Delegation
        {
            Who = delegation.Who.Sub,
            Whom = delegation.Whom.Sub,
            From = delegation.From,
            To = delegation.To,
        };
    }

    public static Task<DelegationEntity> GetCurrentDelegation(
        this DbSet<DelegationEntity> delegations,
        string subjectId,
        string applicationName,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return delegations
            .Include(d => d.Who)
            .Include(d => d.Whom)
            .Include(d => d.Application)
            .FirstOrDefaultAsync(
                d =>
                    d.Selected &&
                    d.From <= now && d.To >= now &&
                    d.Whom.Sub == subjectId &&
                    d.Application.Name == applicationName,
                cancellationToken);
    }

    public static Policy To(this PolicyEntity policy)
    {
        if (policy is null)
        {
            return null;
        }

        return new Policy
        {
            Name = policy.Name,
            Content = policy.Content
        };
    }
}