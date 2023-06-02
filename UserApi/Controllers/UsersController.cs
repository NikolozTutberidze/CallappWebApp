using LogicLayer.Dtos;
using LogicLayer.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto userDto)
        {
            var serviceResponse = await _service.AddUserAsync(userDto);

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

        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var serviceResponse = await _service.GetUserByIdAsync(id);

            if (serviceResponse.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

                return NotFound();
            }

            _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl() + "/" + serviceResponse.Data.Id);

            return Ok(serviceResponse.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var serviceResponse = await _service.GetUsersAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.NoContent)
            {
                _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

                return NoContent();
            }

            _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

            return Ok(serviceResponse.Data);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto userDto)
        {
            var serviceResponse = await _service.UpdateUserAsync(id, userDto);

            if (serviceResponse.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

                return NotFound();
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

            return Ok(serviceResponse.Data);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var serviceResponse = await _service.DeleteUserAsync(id);

            if (serviceResponse.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

                return NotFound();
            }

            _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

            return NoContent();
        }
    }
}
