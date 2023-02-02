using System;
using System.Threading;
using System.Threading.Tasks;

using Balea.Abstractions;
using Balea.Model;
using Balea.Provider.Configuration.Model;
using Balea.Store.Configuration.Options;

namespace Balea.Store.Configuration
{
	public class DelegationStore : IDelegationStore<Delegation>
	{
		private readonly StoreOptions _options;
		private readonly IAppContextAccessor _contextAccessor;

		public DelegationStore(
			StoreOptions options,
			IAppContextAccessor contextAccessor)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
		}

		public Task<AccessResult> CreateAsync(Delegation delegation, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			application.Delegations.Add(delegation);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> UpdateAsync(Delegation delegation, CancellationToken cancellationToken)
		{
			return Task.FromResult(AccessResult.Success);
		}

		public Task<AccessResult> DeleteAsync(Delegation delegation, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			application.Delegations.Remove(delegation);

			return Task.FromResult(AccessResult.Success);
		}

		public Task<Delegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
		{
			var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
			var delegation = application.Delegations.GetCurrentDelegation(subject);

			return Task.FromResult(delegation);
		}

		public Task<string> GetWhoAsync(Delegation delegation, CancellationToken cancellationToken)
		{
			return Task.FromResult(delegation.Who);
		}

		public Task<string> GetWhomAsync(Delegation delegation, CancellationToken cancellationToken)
		{
			return Task.FromResult(delegation.Whom);
		}

		public Task<DateTime> GetFromAsync(Delegation delegation, CancellationToken cancellationToken)
		{
			return Task.FromResult(delegation.From);
		}

		public Task<DateTime> GetToAsync(Delegation delegation, CancellationToken cancellationToken)
		{
			return Task.FromResult(delegation.To);
		}
	}
}
