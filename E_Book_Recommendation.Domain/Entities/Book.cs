using System.ComponentModel.DataAnnotations.Schema;

namespace E_Book_Recommendation.Domain.Entities
{
    public class Book: BaseEntity<long>
    {
        public string Title { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
        public long AuthorId { get; set; }
        public string Genre { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
