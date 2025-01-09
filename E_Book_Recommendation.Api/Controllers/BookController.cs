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
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public BookController(IMapper mapper, IBookService bookService)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AuthorDto>>), 200)]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _bookService.GetAllBooksAsync());
        }

        [HttpPost("AddBook")]
        [ProducesResponseType(typeof(ApiResponse<List<BookResponse>>), 200)]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            //await _bookService.AddBookAsync(bookDto);
            //return CreatedAtAction(nameof(GetAllBooks), null);
            var response = await _bookService.AddBookAsync(bookDto).ConfigureAwait(false);
            return CustomResponse(response);
        }
    }
}

