using Microsoft.AspNetCore.Mvc;

namespace MvcLogin.Controllers
{
    public class TrangChuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
