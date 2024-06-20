using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace InsecurelySharp.Data
{
    public class ArticlesDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public ArticlesDbContext(DbContextOptions<ArticlesDbContext> options)
            : base(options)
        {

        }

        public ArticlesDbContext()
        {

        }

        public IQueryable<Article> GetArticle(string id)
        {
            return Articles.FromSqlRaw("SELECT * FROM Articles WHERE id=" + id);
        }

        public IQueryable<Article> GetArticleSafe(string id)
        {
            return Articles.FromSqlRaw("SELECT * FROM Articles WHERE id={0}", id);
        }

        public IQueryable<Article> SearchArticle(string text)
        {
            return Articles.FromSqlRaw($"SELECT * FROM Articles WHERE title LIKE '{text}'");
        }

        public IQueryable<Article> SearchArticleSafe(string text)
        {
            return Articles.FromSqlRaw("SELECT * FROM Articles WHERE title LIKE '{0}'", text);
        }
    }

    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}