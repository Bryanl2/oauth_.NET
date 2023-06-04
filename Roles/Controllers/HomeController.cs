using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Roles.Controllers
{
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public string Index() => "Index route";

        [HttpGet("/secret")]
        [Authorize(Roles="admin")]
        public string Secret() => 
            "Secret route";
       
            
        
    }
}
