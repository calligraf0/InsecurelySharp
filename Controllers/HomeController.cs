using InsecurelySharp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InsecurelySharp.Controllers
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

        public IActionResult Vulnerabilities()
        {
            return View();
        }

        //Slide 1
        public IActionResult SecureCoding()
        {
            return View();
        }

        //Slide 2
        public IActionResult SecureCoding2()
        {
            return View();
        }

        //Slide 3
        public IActionResult SecurityCheatsheet()
        {
            return View();
        }

        //Slide 4
        public IActionResult GeneralTips()
        {
            return View();
        }

        //Slide 5
        public IActionResult IntroducingOWASP()
        {
            return View();
        }

        //Slide 6
        public IActionResult TheOWASPTopTen()
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