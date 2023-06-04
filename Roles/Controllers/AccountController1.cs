using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Roles.Controllers
{
    public class AccountController1 : ControllerBase
    {
        [HttpGet("login")]
        public IActionResult Login() =>
            SignIn(new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString()),
                            new Claim("my_role_claim_meditation","admin"),
                        },
                        "cookie",
                        nameType: null,
                        roleType: "my_role_claim_meditation"
                        )
                    ),
                    authenticationScheme:"cookie"
                );
        
    }
}
