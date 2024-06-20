using InsecurelySharp.Models;
using InsecurelySharp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InsecurelySharp.Controllers
{
    public class SQLiController : Controller
    {
        private readonly ILogger<SQLiController> _logger;
        private readonly ArticlesDbContext _context;
        public SQLiController(ILogger<SQLiController> logger, ArticlesDbContext context)
        {
            _logger = logger;
            _context = context;
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

        public IResult Query([FromQuery] string uuid)
        {
            List<Article> articles = new List<Article>();

            _context.GetArticle(uuid).ToList().ForEach(article => {
                Console.WriteLine(article.Id);
                Console.WriteLine(article.Title);
                Console.WriteLine(article.Content);
                articles.Add(article);
            });

            return Results.Ok(new { count = articles.Count, results = articles });
        }

        public IResult QuerySafe([FromQuery] string uuid)
        {
            //Always Log requests if they do not contain personal information so we can identify attacks
            _logger.Log(LogLevel.Information, uuid);

            List<Article> articles = new List<Article>();

            /*To solve the problem we can either use the entity Linq expressions
            Console.WriteLine("Using Linq...");
            _context.Articles.Where(article => article.Id.Equals(int.Parse(uuid))).ToList().ForEach(article =>
            {
                Console.WriteLine(article.Id);
                Console.WriteLine(article.Title);
                Console.WriteLine(article.Content);
                articles.Add(article);
            });*/
            /*or parametrized queries*/
            _context.GetArticleSafe(uuid).ToList().ForEach(article =>
            {
                Console.WriteLine(article.Id);
                Console.WriteLine(article.Title);
                Console.WriteLine(article.Content);
                articles.Add(article);
            });


            return Results.Ok(new { count = articles.Count, results = articles });
        }

        public IResult Search([FromQuery] string text)
        {
            List<Article> articles = new List<Article>();

            _context.SearchArticle(text).ToList().ForEach(article => {
                Console.WriteLine(article.Id);
                Console.WriteLine(article.Title);
                Console.WriteLine(article.Content);
                articles.Add(article);
            });

            return Results.Ok(new { count = articles.Count, results = articles });
        }

        public IResult SearchSafe([FromQuery] string text)
        {
            List<Article> articles = new List<Article>();

            //See solution in SearchArticleSafe method
            _context.SearchArticleSafe(text).ToList().ForEach(article => {
                Console.WriteLine(article.Id);
                Console.WriteLine(article.Title);
                Console.WriteLine(article.Content);
                articles.Add(article);
            });

            return Results.Ok(new { count = articles.Count, results = articles });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}