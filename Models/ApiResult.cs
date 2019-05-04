using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPCServer.Models
{
    [System.Serializable]
    public enum Result
    {
        Ok,
        Failed,
        BadRequest
    };

    [System.Serializable]
    public class ApiResult
    {
        public Result result;
        public string message;

        public static ApiResult DefaultError
        {
            get
            {
                return new ApiResult(Result.Failed, "An unexpected error occurred.");
            }
        }

        public ApiResult(Result result, string message)
        {
            this.result = result;
            this.message = message;
        }
        public ApiResult()
        {

        }
        
    }
}
