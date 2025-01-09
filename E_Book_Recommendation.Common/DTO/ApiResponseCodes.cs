using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Common.DTO
{
    public enum ApiResponseCodes
    {
        [Description("Success")]
        Ok = 0,
        [Description("Validation Error")]
        ValidationError = 1,
        [Description("Not Found")]
        NotFound = 2,
        [Description("Bad Request")]
        ProcessingError = 3,
        [Description("Unauthorized Access")]
        AuthorizationError = 4,
        [Description("Exception Occurred")]
        Exception = 5
    }
}
