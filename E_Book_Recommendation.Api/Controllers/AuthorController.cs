using AutoMapper;
using E_Book_Recommendation.Business.Logic.Implementation;
using E_Book_Recommendation.Business.Logic.Interface;
using E_Book_Recommendation.Common.DTO;
using E_Book_Recommendation.Common.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_Book_Recommendation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ExecutionResult<>), 400)]
    public class AuthorController : BaseController
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _Mapper;
       // private readonly ILo _LogService;

        public AuthorController( IAuthorService author,IMapper mapper)
        {
          _authorService = author;
            _Mapper = mapper;
        }

        [HttpGet("GetAuthors")]
        [ProducesResponseType(typeof(ApiResponse<List<AuthorResponse>>), 200)]
        public async Task<IActionResult> GetAllAuthor()
        {
            return Ok(await _authorService.GetAllAuthorsAsync());
        }

        [HttpPost("AddAuthor")]
        [ProducesResponseType(typeof(ApiResponse<List<AuthorResponse>>), 200)]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorDto authorDto)
        {
            var response = await _authorService.AddAuthorAsync(authorDto).ConfigureAwait(false);
            return CustomResponse(response);
        }


    }
}
