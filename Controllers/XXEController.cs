using InsecurelySharp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace InsecurelySharp.Controllers
{
    public class XXEController : Controller
    {
        private readonly ILogger<XXEController> _logger;

        public XXEController(ILogger<XXEController> logger)
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
        public IActionResult Solution() {
            return View();
        }

        public IResult XMLProcess([FromForm]string xml)
        {
            //https://dotnetcoretutorials.com/2021/01/11/testing-xxe-vulnerabilities-in-net-core/
            //https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca3075
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.XmlResolver = new XmlUrlResolver();

            try {
                xmlDoc.LoadXml(xml);
            } catch(System.Xml.XmlException e) {
                return Results.UnprocessableEntity(e.Message);
            }

            return Results.Ok(xmlDoc.InnerText);
        }

        public IResult XMLProcessSafe([FromForm] string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.XmlResolver = null; // This is the default on newer versions of .NET

            try {
                xmlDoc.LoadXml(xml);
            } catch (System.Xml.XmlException e) {
                return Results.UnprocessableEntity(e.Message);
            }

            return Results.Ok(xmlDoc.InnerText);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
