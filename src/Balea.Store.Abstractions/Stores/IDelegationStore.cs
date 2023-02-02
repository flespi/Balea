using System;
using System.Threading;
using System.Threading.Tasks;

namespace Balea.Store
{
	public interface IDelegationStore<TDelegation>
		where TDelegation : class
	{
		Task<AccessResult> CreateAsync(TDelegation delegation, CancellationToken cancellationToken);
		Task<AccessResult> UpdateAsync(TDelegation delegation, CancellationToken cancellationToken);
		Task<AccessResult> DeleteAsync(TDelegation delegation, CancellationToken cancellationToken);

		Task<TDelegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken);

		Task<string> GetWhoAsync(TDelegation delegation, CancellationToken cancellationToken);
		Task<string> GetWhomAsync(TDelegation delegation, CancellationToken cancellationToken);
		Task<DateTime> GetFromAsync(TDelegation delegation, CancellationToken cancellationToken);
		Task<DateTime> GetToAsync(TDelegation delegation, CancellationToken cancellationToken);
	}
}
