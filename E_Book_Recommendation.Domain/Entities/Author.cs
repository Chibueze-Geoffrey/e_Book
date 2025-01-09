namespace E_Book_Recommendation.Domain.Entities
{
    public class Author : BaseEntity<long>
    {
        public string AuthorName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
