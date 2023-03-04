using BB_RPM_PROJEKT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BB_RPM_PROJEKT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string log, string pass)
        {
            try
            {
                return Redirect("/Home/News");
            }
            catch
            {
                ViewBag.H = "error";
            }
            return View();
        }

        public IActionResult Disciplins()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Raspisanie()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}