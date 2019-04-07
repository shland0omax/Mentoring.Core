using Microsoft.AspNetCore.Mvc;

namespace Mentoring.Core.Module1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}