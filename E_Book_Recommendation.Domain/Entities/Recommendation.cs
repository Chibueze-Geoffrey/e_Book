using System.ComponentModel.DataAnnotations.Schema;

namespace E_Book_Recommendation.Domain.Entities
{
    public class Recommendation: BaseEntity<long>
    {
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public long BookId { get; set; }
        public string RecommendedBy { get; set; }
        public DateTime RecommendedDate { get; set; }
    }
}
