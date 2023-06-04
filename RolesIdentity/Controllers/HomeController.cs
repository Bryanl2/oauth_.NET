using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RolesIdentity.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public string Index() => "Index route";

        [HttpGet("/secret")]
        [Authorize(Roles = "admin")]
        public string Secret() =>
            "Secret route";

    }
}
