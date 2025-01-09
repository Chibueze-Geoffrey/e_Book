using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Common.DTO.Response
{
    public class BookResponse
    {
        public string Title { get; set; }
        public long AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Genre { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
