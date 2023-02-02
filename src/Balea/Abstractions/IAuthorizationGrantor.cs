using Balea.Model;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Balea.Abstractions
{
    public interface IAuthorizationGrantor
    {
        Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
        Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default);
    }
}
