using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse(string[] errors) : base(400)
        {
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; private set; }
    }
}
