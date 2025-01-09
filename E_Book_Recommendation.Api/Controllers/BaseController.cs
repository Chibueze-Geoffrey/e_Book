using E_Book_Recommendation.Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Book_Recommendation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult ApiResponse<T>(
        T data = default(T),
        string[] message = null,
        ApiResponseCodes codes = ApiResponseCodes.Ok,
        int? totalCount = 0) where T : class
        {
            ApiResponse<T> response = new ApiResponse<T>
            {
                Result = data,
                Response = codes,
                ValidationMessages = message?.ToList()
            };

            response.RetryCount = totalCount ?? 0;
            return GenerateHttpResponse(codes, response);
        }

        public IActionResult ApiResponse<T>(
            T data = default(T),
            string message = "",
            ApiResponseCodes codes = ApiResponseCodes.Ok,
            int? totalCount = 0) where T : class
        {
            ApiResponse<T> response = new ApiResponse<T>
            {
                Message = message,
                Result = data,
                Response = codes
            };

            response.RetryCount = totalCount ?? 0;
            return GenerateHttpResponse(codes, response);
        }

        /// <summary>
        /// Handles ApiResponse and maps it to appropriate HTTP response codes.
        /// </summary>
        /// <typeparam name="T">Type of the response result</typeparam>
        /// <param name="result">ApiResponse object</param>
        /// <returns>IActionResult with appropriate status code</returns>
        protected IActionResult CustomResponse<T>(ApiResponse<T> result) where T : class
        {
            return result switch
            {
                { Response: ApiResponseCodes.Ok } => Ok(result),
                { Response: ApiResponseCodes.NotFound } => NotFound(result),
                { Response: ApiResponseCodes.AuthorizationError } => Unauthorized(result),
                { Response: ApiResponseCodes.Exception } => StatusCode(StatusCodes.Status500InternalServerError, result),
                _ => BadRequest(result) //ValidationError, ProcessingError
            };
        }

        protected IActionResult CustomErrorResponse(string errors)
        {
            var response = new ApiResponse
            {
                Response = ApiResponseCodes.ProcessingError,
                Message = errors
            };
            return BadRequest(response);
        }

        protected ApiResponse<IEnumerable<string>> GetModelStateValidationErrorsAsList()
        {
            var response = new ApiResponse<IEnumerable<string>>
            {
                Response = ApiResponseCodes.ProcessingError
            };
            var errors = ModelState.Values
                .SelectMany(a => a.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            response.Result = errors;
            return response;
        }

        /// <summary>
        /// Maps ApiResponseCodes to HTTP status codes.
        /// </summary>
        private IActionResult GenerateHttpResponse<T>(ApiResponseCodes codes, ApiResponse<T> response) where T : class
        {
            return codes switch
            {
                ApiResponseCodes.Exception => StatusCode(StatusCodes.Status500InternalServerError, response),
                ApiResponseCodes.AuthorizationError => Unauthorized(response),
                ApiResponseCodes.NotFound => NotFound(response),
                ApiResponseCodes.ProcessingError => BadRequest(response),
                ApiResponseCodes.ValidationError => BadRequest(response),
                ApiResponseCodes.Ok => Ok(response),
                _ => Ok(response)
            };
        }

    }
}
