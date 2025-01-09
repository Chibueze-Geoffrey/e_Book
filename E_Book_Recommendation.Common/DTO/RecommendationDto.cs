using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Common.DTO
{
    public class RecommendationDto
    {
        public string BookTitle { get; set; }
        public string RecommendedBy { get; set; }
        public DateTime RecommendedDate { get; set; }
    }
}
