using InsecurelySharp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InsecurelySharp.Controllers
{
    public class LoggingSensitiveInformationController : Controller
    {
        private readonly ILogger<LoggingSensitiveInformationController> _logger;
        public LoggingSensitiveInformationController(ILogger<LoggingSensitiveInformationController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Exercise()
        {
            return View();
        }

        public IResult Login([FromQuery]string username, [FromQuery] string password)
        {
            _logger.LogInformation($"LoginAttempt: {username} {password}");
            _logger.LogInformation($"Login: Success.");

            return Results.Ok();
        }

        public IResult LoginSafe([FromBody] string username, [FromBody] string password)
        {
            _logger.LogInformation($"LoginAttempt: {username}");
            _logger.LogInformation($"Login: Success.");
            
            return Results.Ok();
        }

        public IActionResult Solution()
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