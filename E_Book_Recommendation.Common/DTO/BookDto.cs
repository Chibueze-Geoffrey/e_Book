using System.ComponentModel.DataAnnotations.Schema;

namespace E_Book_Recommendation.Common.DTO
{
    public class BookDto
    {
        public string Title { get; set; }
        public long AuthorId { get; set; }
        public string Genre { get; set; }
        public DateTime PublishedAt { get; set; }
    }
   
}
