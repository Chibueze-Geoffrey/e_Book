using AutoMapper;
using E_Book_Recommendation.Business.Logic.Interface;
using E_Book_Recommendation.Common.DTO;
using E_Book_Recommendation.Common.DTO.Response;
using E_Book_Recommendation.Data.DataAccess.DataAccesInterfaces;
using E_Book_Recommendation.Data.UnitOfWork_;
using E_Book_Recommendation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Business.Logic.Implementation
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _Mapper = mapper;
        }

        public async Task<ApiResponse<BookResponse>> AddBookAsync(BookDto bookDto)
        {
            try
            {
                var book = _Mapper.Map<Book>(bookDto);

                // Make sure the author exists
                var author = await _UnitOfWork.Repository<Author>().GetByIdAsync(bookDto.AuthorId);
                if (author == null)
                {
                    throw new Exception("Author not found.");
                }

                book.Author = author; // Assign the author to the book

                await _UnitOfWork.Repository<Book>().AddAsync(book);
                await _UnitOfWork.Commit();

                // Map the added book to BookResponse
                var bookResponse = _Mapper.Map<BookResponse>(book);

                var response = new ApiResponse<BookResponse>
                {
                    Response = ApiResponseCodes.Ok,
                    Result = bookResponse
                };

                return response;
            }
            catch (Exception ex)
            {
                // Handle exception (logging, etc.)
                return ApiResponse<BookResponse>.Failed(ex.Message);
            }
        }


        public async Task<ApiResponse<IEnumerable<BookResponse>>> GetAllBooksAsync()
        {
            var response = ApiResponse<IEnumerable<BookResponse>>.Failed(string.Empty);

            try
            {
                // Retrieve all Books from the repository, including their Authors
                var books = await _UnitOfWork.Repository<Book>()
                                             .GetAllIncludingAsync(b => b.Author);

                // Map the books to BookResponse
                var bookResponses = _Mapper.Map<IEnumerable<BookResponse>>(books);

                // Create a successful ApiResponse
                response = new ApiResponse<IEnumerable<BookResponse>>
                {
                    Response = ApiResponseCodes.Ok,
                    Result = bookResponses
                };

                return response;
            }
            catch (Exception ex)
            {
                // Handle exception (logging, etc.)
                response.Message = ex.Message;
                return response;
            }
        }

    }
}
