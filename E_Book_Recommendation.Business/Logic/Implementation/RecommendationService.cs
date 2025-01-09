using AutoMapper;
using E_Book_Recommendation.Business.Logic.Interface;
using E_Book_Recommendation.Common.DTO;
using E_Book_Recommendation.Data.DataAccess.DataAccesInterfaces;
using E_Book_Recommendation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Business.Logic.Implementation
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;
        public RecommendationService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _Mapper = mapper;
        }
        public async Task<IEnumerable<RecommendationDto>> GetRecommendationsAsync()
        {
            var GetRecommendations = await _UnitOfWork.Repository<Recommendation>().GetAllAsync();
           return  _Mapper.Map<IEnumerable<RecommendationDto>>(GetRecommendations);
        }
    }
}
