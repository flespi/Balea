using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Balea.Abstractions;
using Balea.Provider.EntityFrameworkCore.DbContexts;
using Balea.Provider.EntityFrameworkCore.Entities;

using Microsoft.EntityFrameworkCore;

namespace Balea.Store.EntityFrameworkCore;

public class PolicyStore : IPolicyStore<PolicyEntity>
{
    private readonly BaleaDbContext _context;
    private readonly IAppContextAccessor _contextAccessor;

    public PolicyStore(
        BaleaDbContext context,
        IAppContextAccessor contextAccessor)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public async Task<PolicyEntity> FindByNameAsync(string policyName, CancellationToken cancellationToken)
    {
        return await _context.Policies
            .Where(x => x.Application.Name == _contextAccessor.AppContext.Name)
            .Where(x => x.Name == policyName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AccessResult> CreateAsync(PolicyEntity policy, CancellationToken cancellationToken)
    {
        await _context.Policies.AddAsync(policy);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> UpdateAsync(PolicyEntity policy, CancellationToken cancellationToken)
    {
        _context.Policies.Update(policy);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> DeleteAsync(PolicyEntity policy, CancellationToken cancellationToken)
    {
        _context.Policies.Remove(policy);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public Task<string> GetContentAsync(PolicyEntity policy, CancellationToken cancellationToken)
    {
        return Task.FromResult(policy.Content);
    }
}