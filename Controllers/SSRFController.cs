using InsecurelySharp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace InsecurelySharp.Controllers
{
    public class SSRFController : Controller
    {
        private readonly ILogger<SSRFController> _logger;
        private readonly List<string> _userfiles = new List<string>();

        public SSRFController(ILogger<SSRFController> logger)
        {
            _logger = logger;
            _userfiles.Add("file1.jpg");
            _userfiles.Add("file2.jpg");
            _userfiles.Add("file3.txt");
            _userfiles.Add("file4.txt");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Exercise()
        {
            return View();
        }

        public IActionResult Solution()
        {
            return View();
        }

        public async Task<IResult> Get([FromQuery]string url)
        {
            var net = new HttpClient();
            var res = await net.GetAsync(url);
            var content = await res.Content.ReadAsByteArrayAsync();

            _logger.LogInformation("Grabbed: " + url);
            var contentType = "APPLICATION/octet-stream";

            return Results.Ok(new { file = File(content, contentType, url) });
        }

        public async Task<IResult> GetSafe([FromQuery] string resource)
        {
            if (!_userfiles.Contains(resource)) {
                return Results.Forbid();
            }

            var net = new HttpClient();
            var res = await net.GetAsync($"http://127.0.0.1:8000/{resource}");
            var content = await res.Content.ReadAsByteArrayAsync();
            _logger.LogInformation("Grabbed: " + resource);

            var contentType = "APPLICATION/octet-stream";
            
            return Results.Ok( new { file = File(content, contentType, resource) });


            /*
            From stackoverflow, analyze:
            [FromQuery] string pdfGuid
            ...
            uri = new Uri(generatePdfsRetrieveUrl + pdfGuid + ".pdf");
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(uri);
            using (var fs = new FileStream(
                HostingEnvironment.MapPath(string.Format("~/Downloads/{0}.pdf", pdfGuid)),
                FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
            }
            //is it safe? if yes why? if no explain.
            */
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}