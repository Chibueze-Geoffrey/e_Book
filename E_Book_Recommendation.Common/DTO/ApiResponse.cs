using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Common.DTO
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Response = ApiResponseCodes.Ok;
            Message = "Success";
        }
        public ApiResponseCodes Response { get; set; }
        public string Message { get; set; }
        public int RetryCount { get; set; }
        public IEnumerable<string> ValidationMessages { get; set; } = new List<string>();
        public bool HasValidationMessages => ValidationMessages.Any();

        public static ApiResponse Failed(string message)
        {
            return new ApiResponse { Message = message, Response = ApiResponseCodes.ProcessingError };
        }

        public static ApiResponse Success(string message)
        {
            return new ApiResponse { Message = message, Response = ApiResponseCodes.Ok };
        }
    }
    public class ApiResponse<T> : ApiResponse//where T : class
    {
        public T Result { get; set; }

        public static ApiResponse<T> Success(T result, string message = "success")
        {
            var response = new ApiResponse<T> { Response = ApiResponseCodes.Ok, Result = result, Message = message };

            return response;
        }

        /// <summary>
        /// Creates a failed result. It takes no result object
        /// </summary>
        /// <param name="errorMessage">The error message returned with the response</param>
        /// <returns>The created response object</returns>
        public static new ApiResponse<T> Failed(string errorMessage)
        {
            var response = new ApiResponse<T> { Response = ApiResponseCodes.ProcessingError, Message = errorMessage };

            return response;
        }

        public static ApiResponse<T> NotFound(string errorMessage)
        {
            var response = new ApiResponse<T> { Response = ApiResponseCodes.NotFound, Message = errorMessage };

            return response;
        }
    }

    public class PagedList<T> where T : class
    {
        public T Data { get; set; }
        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}
