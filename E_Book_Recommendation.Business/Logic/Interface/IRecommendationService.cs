using E_Book_Recommendation.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Business.Logic.Interface
{
    public interface IRecommendationService
    {
        Task<IEnumerable<RecommendationDto>> GetRecommendationsAsync();
    }
}
