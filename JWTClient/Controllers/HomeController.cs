using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JWTClient.Controllers
{
    public class HomeController : Controller
    {
        public async  Task<ActionResult> Index()
        {

            IEnumerable<Claim> claims = null;
            try
            {
                claims = await JWTServices.GetTokenAsync("username", "password").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                claims = null;
                throw e;
            }

            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
