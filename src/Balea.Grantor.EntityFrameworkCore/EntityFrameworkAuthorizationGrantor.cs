using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Balea.Abstractions;
using Balea.Model;
using Balea.Provider.EntityFrameworkCore.DbContexts;
using Balea.Provider.EntityFrameworkCore.Entities;

using Microsoft.EntityFrameworkCore;

namespace Balea.Grantor.EntityFrameworkCore;

public class EntityFrameworkAuthorizationGrantor<TContext> : IAuthorizationGrantor
    where TContext : BaleaDbContext
{
    private readonly TContext _context;
    private readonly BaleaOptions _options;
    private readonly IAppContextAccessor _contextAccessor;

    public EntityFrameworkAuthorizationGrantor(TContext context, BaleaOptions options, IAppContextAccessor contextAccessor)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public async Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        var sourceRoleClaims = user.GetClaimValues(_options.ClaimTypeMap.RoleClaimType);
        var delegation = await _context.Delegations.GetCurrentDelegation(
            user.GetSubjectId(_options),
            _contextAccessor.AppContext.Name,
            cancellationToken);
        var subject = GetSubject(user, delegation);
        var roles = await _context.Roles
                .AsNoTracking()
                .Include(r => r.Application)
                .Include(r => r.Mappings)
                .ThenInclude(rm => rm.Mapping)
                .Include(r => r.Subjects)
                .ThenInclude(rs => rs.Subject)
                .Include(r => r.Permissions)
                .ThenInclude(rp => rp.Permission)
                .Where(role =>
                    role.Application.Name == _contextAccessor.AppContext.Name &&
                    role.Enabled &&
                    (role.Subjects.Any(rs => rs.Subject.Sub == subject) || role.Mappings.Any(rm => sourceRoleClaims.Contains(rm.Mapping.Name)))
                )
                .ToListAsync(cancellationToken);

        return new AuthorizationContext
        {
            Roles = roles.Select(r => r.To()).ToList(),
            Delegation = delegation.To(),
        };
    }

    public async Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default)
    {
        var policy = await _context
            .Policies
            .Include(p => p.Application)
            .SingleOrDefaultAsync(p => p.Application.Name == _contextAccessor.AppContext.Name && p.Name == name);

        return policy.To();
    }

    private string GetSubject(ClaimsPrincipal user, DelegationEntity delegation)
    {
        return delegation?.Who?.Sub ?? user.GetSubjectId(_options);
    }
}