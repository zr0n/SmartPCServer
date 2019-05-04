using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SmartPCServer.Models;
using SmartPCServer.Services;

namespace SmartPCServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {

        private readonly ApiService _service;

        //Inject by Dependency Injection
        public MainController(IApiService service)
        {
            _service = (ApiService) service;
        }

        [HttpGet]
        [Route("/")]
        public ActionResult<ApiResult> Index()
        {

            var result = new ApiResult();
            result.result = Result.Ok;
            result.message = "Everything is awesome.";

            return _service.DoSomething();
            return result;
            return Ok();
        }

    }
}