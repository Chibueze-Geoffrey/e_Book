using E_Book_Recommendation.Common.DTO;
using E_Book_Recommendation.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Business.Logic.Interface
{
    public interface IBookService
    {
        Task<ApiResponse<IEnumerable<BookResponse>>> GetAllBooksAsync();
        Task<ApiResponse<BookResponse>> AddBookAsync(BookDto bookDto);
    }
}
