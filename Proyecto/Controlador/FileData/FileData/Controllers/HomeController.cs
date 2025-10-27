using Microsoft.AspNetCore.Mvc;

namespace FileData.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
