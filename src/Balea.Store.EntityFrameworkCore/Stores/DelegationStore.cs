using System;
using System.Threading;
using System.Threading.Tasks;

using Balea.Abstractions;
using Balea.Provider.EntityFrameworkCore.DbContexts;
using Balea.Provider.EntityFrameworkCore.Entities;

using Microsoft.EntityFrameworkCore;

namespace Balea.Store.EntityFrameworkCore;

public class DelegationStore : IDelegationStore<DelegationEntity>
{
    private readonly BaleaDbContext _context;
    private readonly IAppContextAccessor _contextAccessor;

    public DelegationStore(
        BaleaDbContext context,
        IAppContextAccessor contextAccessor)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public async Task<AccessResult> CreateAsync(DelegationEntity delegation, CancellationToken cancellationToken)
    {
        await _context.Delegations.AddAsync(delegation);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> UpdateAsync(DelegationEntity delegation, CancellationToken cancellationToken)
    {
        _context.Delegations.Update(delegation);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> DeleteAsync(DelegationEntity delegation, CancellationToken cancellationToken)
    {
        _context.Delegations.Remove(delegation);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public Task<DelegationEntity> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var delegations = _context.Delegations
            .Include(d => d.Who)
            .Include(d => d.Whom)
            .Include(d => d.Application)
            .FirstOrDefaultAsync(
                d =>
                    d.Selected &&
                    d.From <= now && d.To >= now &&
                    d.Whom.Sub == subject &&
                    d.Application.Name == _contextAccessor.AppContext.Name,
                cancellationToken);

        return delegations;
    }

    public Task<string> GetWhoAsync(DelegationEntity delegation, CancellationToken cancellationToken)
    {
        return Task.FromResult(delegation.Who.Sub);
    }

    public Task<string> GetWhomAsync(DelegationEntity delegation, CancellationToken cancellationToken)
    {
        return Task.FromResult(delegation.Whom.Sub);
    }

    public Task<DateTime> GetFromAsync(DelegationEntity delegation, CancellationToken cancellationToken)
    {
        return Task.FromResult(delegation.From);
    }

    public Task<DateTime> GetToAsync(DelegationEntity delegation, CancellationToken cancellationToken)
    {
        return Task.FromResult(delegation.To);
    }
}