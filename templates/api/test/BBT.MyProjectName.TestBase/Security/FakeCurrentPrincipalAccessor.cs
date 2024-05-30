using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

namespace BBT.MyProjectName.Security;

public class FakeCurrentPrincipalAccessor
{
    protected ClaimsPrincipal GetClaimsPrincipal()
    {
        return (Thread.CurrentPrincipal as ClaimsPrincipal)!;
    }

    private ClaimsPrincipal GetPrincipal()
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim("sub", "2e701e62-0953-4dd3-910b-dc6cc93ccb0d"),
            new Claim("username", "bbt-test"),
            new Claim("email", "bbt-test@burgan.com.tr")
        }));
    }
}
