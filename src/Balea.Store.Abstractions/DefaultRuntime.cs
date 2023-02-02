using System;

using Balea.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace Balea.Store
{
	public class StoreAuthorizationGrantorFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public StoreAuthorizationGrantorFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public IAuthorizationGrantor Create()
		{
			var options = _serviceProvider.GetRequiredService<BaleaStoreOptions>();

			var serviceType = typeof(StoreAuthorizationGrantor<,,>).MakeGenericType(
				options.DelegationType,
				options.PolicyType,
				options.RoleType);

			return (IAuthorizationGrantor)ActivatorUtilities.CreateInstance(_serviceProvider, serviceType);
		}
	}
}
