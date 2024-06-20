using InsecurelySharp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InsecurelySharp.Controllers
{
    public class XSSController : Controller
    {
        private readonly ILogger<XSSController> _logger;

        public XSSController(ILogger<XSSController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Exercise([FromQuery]string name)
        {
            if (name == null) {
                ViewData["name"] = "user";
            } else {
                ViewData["name"] = name;
            }
            return View();
        }

        public IActionResult Solution([FromQuery] string name)
        {
            if (name == null) {
                ViewData["name"] = "user";
            } else {
                ViewData["name"] = name;
            }
            return View();
        }

        public IResult Reflected()
        {
            return Results.Ok();
        }

        public IResult ReflectedSafe()
        {
            return Results.Ok();
        }
        public IResult Stored()
        {
            return Results.Ok();
        }

        public IResult StoredSafe()
        {
            return Results.Ok();
        }

        public IResult DOM()
        {
            return Results.Ok();
        }

        public IResult DOMSafe()
        {
            return Results.Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}