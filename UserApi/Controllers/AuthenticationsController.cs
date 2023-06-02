using LogicLayer.Dtos;
using LogicLayer.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<AuthenticationsController> _logger;

        public AuthenticationsController(IUserService service, ILogger<AuthenticationsController> logger)
        {
            _service = service;
            _logger = logger;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AddUserDto request)
        {
            var serviceResponse = await _service.RegisterAsync(request);

            if (serviceResponse.StatusCode == HttpStatusCode.Conflict)
            {
                _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + serviceResponse.Message);

                return Conflict(serviceResponse.Message);
            }

            if (serviceResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + serviceResponse.Message);

                return BadRequest(serviceResponse.Message);
            }

            _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl() + "/" + serviceResponse.Data.Id);

            return Created(Request.GetEncodedUrl() + "/" + serviceResponse.Data.Id, serviceResponse.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var serviceResponse = await _service.LoginAsync(request);

            if (serviceResponse.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

                return NotFound();
            }

            return Ok(serviceResponse.Data);
        }
    }
}
