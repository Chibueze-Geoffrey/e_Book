using E_Book_Recommendation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Book_Recommendation.Data.DataAccess
{
    public class BookRecommendationDbContext : DbContext
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<Recommendation> Recommendation { get; set; }
        public DbSet<Author> Author { get; set; }

     
        public BookRecommendationDbContext(DbContextOptions<BookRecommendationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


    }
}
