using AutoMapper;
using E_Book_Recommendation.Business.Logic.Interface;
using E_Book_Recommendation.Common.DTO;
using E_Book_Recommendation.Common.DTO.Response;
using E_Book_Recommendation.Data.DataAccess.DataAccesInterfaces;
using E_Book_Recommendation.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Business.Logic.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;
        private readonly ILogger<AuthorService> _Logger;
        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper,ILogger<AuthorService>logger)
        {
            _Mapper = mapper;
            _UnitOfWork = unitOfWork;
            _Logger = logger;
        }
        public async Task<ApiResponse<List<AuthorResponse>>> AddAuthorAsync(AuthorDto authorDto)
        {
            var response = ApiResponse<List<AuthorResponse>>.Failed(string.Empty);

            try
            {
                // Map AuthorDto to Author
                var author = _Mapper.Map<Author>(authorDto);

                // Add the author to the repository
                await _UnitOfWork.Repository<Author>().AddAsync(author);

                // Commit the changes to the database
                await _UnitOfWork.Commit();

                // Map the added author to AuthorResponse
                var authorResponse = _Mapper.Map<AuthorResponse>(author);

                // Create a list with the single author response
                var authorsResponseList = new List<AuthorResponse> { authorResponse };

                // Update the response
                response = new ApiResponse<List<AuthorResponse>>
                {
                    Response = ApiResponseCodes.Ok,
                    Result = authorsResponseList,
                   
                };

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<IEnumerable<AuthorResponse>>> GetAllAuthorsAsync()
        {
            var response = ApiResponse<IEnumerable<AuthorResponse>>.Failed(string.Empty);

            try
            {
                // Retrieve all authors from the repository
                var authors = await _UnitOfWork.Repository<Author>().GetAllAsync();

                // Map authors directly to AuthorResponse
                var authorResponses = _Mapper.Map<IEnumerable<AuthorResponse>>(authors);

                //  successful ApiResponse
                response = new ApiResponse<IEnumerable<AuthorResponse>>
                {
                    Response = ApiResponseCodes.Ok,
                    Result = authorResponses
                };

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }



    }
}
