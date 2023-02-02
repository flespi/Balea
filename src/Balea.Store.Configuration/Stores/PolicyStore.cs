using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Balea.Abstractions;
using Balea.Model;
using Balea.Provider.Configuration.Model;
using Balea.Store.Configuration.Options;

namespace Balea.Store.Configuration
{
	public class PolicyStore : IPolicyStore<Policy>
	{
		private readonly StoreOptions _options;
		private readonly IAppContextAccessor _contextAccessor;

		public PolicyStore(
			StoreOptions options,
			IAppContextAccessor contextAccessor)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
		}

		public Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			var policy = application.Policies.FirstOrDefault(x => x.Name == policyName);

			return Task.FromResult(policy);
		}

		public Task<AccessResult> CreateAsync(Policy policy, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			application.Policies.Add(policy);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> UpdateAsync(Policy policy, CancellationToken cancellationToken)
		{
			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> DeleteAsync(Policy policy, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			application.Policies.Remove(policy);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<string> GetContentAsync(Policy policy, CancellationToken cancellationToken)
		{
			return Task.FromResult(policy.Content);
		}
	}
}
