using InsecurelySharp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace InsecurelySharp.Controllers
{
    public class CommandInjectionController : Controller
    {
        private readonly ILogger<CommandInjectionController> _logger;

        public CommandInjectionController(ILogger<CommandInjectionController> logger)
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

        public IResult Ping([FromQuery] string ip="127.0.0.1")
        {   

            if (ip.Contains(";")) {
                return Results.Forbid();
            }

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe"; 
            startInfo.Arguments = $"/C ping {ip}"; 
            process.StartInfo = startInfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string commandOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return Results.Ok(new { output = commandOutput });
        }

        public IResult PingSafe([FromQuery] string ip = "127.0.0.1")
        {
            /*
             * Always validate the type of data we receive if possible or make sure to escaping it corrrectly:
             * https://cheatsheetseries.owasp.org/cheatsheets/DotNet_Security_Cheat_Sheet.html#os-injection
             */
            System.Net.IPAddress address;
            if (System.Net.IPAddress.TryParse(ip, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        ip = address.ToString();
                        break;

                    default:
                        return Results.Forbid();
                }
            }

            /*
             * Avoid using generic cmd.exe as FileName, run instead just the specific command.
             * Be careful, some programs do more than we expect (see lolbas)
             */
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "ping";
            startInfo.Arguments = $"{ip}";
            process.StartInfo = startInfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string commandOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return Results.Ok(new { output = commandOutput });
        }

        public IActionResult Solution() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}