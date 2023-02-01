using System.Linq;
using System.Security.Claims;

namespace Balea.Extensions.AspNetCore.Hosting;

internal static class Utils
{
	public static ClaimsPrincipal CreatePrincipal(BaleaOptions options, string subject, string[]? roles)
	{
		var claims = Enumerable.Empty<Claim>();

		var subjectType = options.DefaultClaimTypeMap.AllowedSubjectClaimTypes.First();
		var subjectClaim = new Claim(subjectType, subject);

		claims = claims.Append(subjectClaim);

		if (roles is not null)
		{
			var roleType = options.DefaultClaimTypeMap.RoleClaimType;
			var roleClaims = roles.Select(role => new Claim(roleType, role));

			claims = claims.Union(roleClaims);
		}

		var scheme = options.Schemes.FirstOrDefault();

		var identity = new ClaimsIdentity(claims, scheme);
		var principal = new ClaimsPrincipal(identity);

		return principal;
	}
}
