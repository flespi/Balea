using System;

namespace Balea.Abstractions
{
	public interface IAppContextAccessor
	{
		AppContext AppContext { get; set; }
	}
}
