using InsecurelySharp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InsecurelySharp.Controllers
{
    public class PathTraversalController : Controller
    {
        private readonly ILogger<PathTraversalController> _logger;

        public PathTraversalController(ILogger<PathTraversalController> logger)
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

        public IActionResult Solution()
        {
            return View();
        }

        /*
        public IResult Download([FromQuery]string filename) 
        {
            //TODO: allow download of arbitrary files
            return Results.Ok();
        }

        public IResult DownloadSafe([FromQuery] string filename)
        {
            //TODO: via database 
            return Results.Ok();
        }
        */

        public IResult Upload([FromForm]List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            List<string> saved = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = "uploads\\"+formFile.FileName; //Path.Combine("uploads", formFile.FileName); //is it the same?

                    //Check for bad extensions
                    if (filePath.EndsWith(".cshtml") || filePath.EndsWith(".exe")) {
                        return Results.Forbid();
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        formFile.CopyTo(stream);
                        saved.Add(Path.GetFullPath(filePath));
                    }
                }
            }

            return Results.Ok(new { count = files.Count, size, saved=saved });
        }

        public IResult UploadSafe([FromForm] List<IFormFile> files)
        {
            /*
             Do NOT use the FileName property of IFormFile other than for display and logging. 
             When displaying or logging, HTML encode the file name. An attacker can provide a malicious filename, including full paths or relative paths. 
             Applications should:
                - Remove the path from the user-supplied filename.
                - Save the HTML-encoded, path-removed filename for UI or logging.
                - Generate a new random filename for storage.

             To validate file types do not rely on extensions: 
                - Validate the files with a library (e.g. Imagick)
                - Parse the file headers & strip all metadata
             
             Remember to scan uploaded files for malware!
             Also be careful when handling/extracting archives (Zipslips)!
             */
            long size = files.Sum(f => f.Length);
            List<string> saved = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filename = Path.GetFileName(formFile.FileName);
                    _logger.Log(LogLevel.Information, $"Uploaded file: {formFile.FileName} via UploadSafe");
                    filename = Path.GetRandomFileName();

                    var filePath = Path.Combine(Path.GetTempPath(), filename); 

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        formFile.CopyTo(stream);
                        /* 
                            Here once should scan, then move to another safe directory
                        */

                        //saved.Add(filename); // Only return whatever needed to call whatever function we use to retreive the file (if users need to be able to re-download them) 
                                               // possibly some id from a database which links to the resource, 
                                               // obviously always check if user is allowed to download such file!
                    }
                }
            }

            //Avoid the info disclosure
            return Results.Ok(new { count = files.Count, size });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}