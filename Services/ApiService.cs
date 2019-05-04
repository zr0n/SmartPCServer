using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SmartPCServer.Models;
namespace SmartPCServer.Services
{
    public class ApiService : IApiService
    {
        public ApiResult DoSomething()
        {
            return ApiResult.DefaultError;
        }
        

    }
}
