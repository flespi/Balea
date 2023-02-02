using System.Threading;
using System.Threading.Tasks;

namespace Balea.Store
{
	public interface IPolicyStore<TPolicy>
		where TPolicy : class
	{
		Task<TPolicy> FindByNameAsync(string policyName, CancellationToken cancellationToken);

		Task<AccessResult> CreateAsync(TPolicy policy, CancellationToken cancellationToken);
		Task<AccessResult> UpdateAsync(TPolicy policy, CancellationToken cancellationToken);
		Task<AccessResult> DeleteAsync(TPolicy policy, CancellationToken cancellationToken);

		Task<string> GetContentAsync(TPolicy policy, CancellationToken cancellationToken);
	}
}
