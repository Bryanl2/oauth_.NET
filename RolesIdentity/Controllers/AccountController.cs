using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RolesIdentity.Controllers
{
    public class AccountController : ControllerBase
    {
        private SignInManager<IdentityUser> _singInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _singInManager = signInManager;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            _singInManager.PasswordSignInAsync("bryan@test.com", "test", false, false);
            return Ok();
        }


    }
}
