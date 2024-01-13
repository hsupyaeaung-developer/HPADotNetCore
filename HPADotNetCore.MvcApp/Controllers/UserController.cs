using HPADotNetCore.MvcApp;
using Microsoft.AspNetCore.Mvc;

namespace HPADotNetCore.MvcApp.Controllers
{
    public class UserController : Controller
    {
        [ActionName("Index")]
        public IActionResult UserIndex()
        {
            return View("UserIndex");
        }
    }
}
