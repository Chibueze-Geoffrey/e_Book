using E_Book_Recommendation.Common.DTO;
using E_Book_Recommendation.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Business.Logic.Interface
{
    public interface IAuthorService
    {
        Task<ApiResponse<IEnumerable<AuthorResponse>>> GetAllAuthorsAsync();
        Task<ApiResponse<List<AuthorResponse>>> AddAuthorAsync(AuthorDto authordto);
    }
}
