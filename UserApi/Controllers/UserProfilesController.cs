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
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _service;
        private readonly ILogger<UserProfilesController> _logger;

        public UserProfilesController(IUserProfileService service, ILogger<UserProfilesController> logger)
        {
            _service = service;
            _logger = logger;

        }

        [HttpPost]
        public async Task<IActionResult> AddProfile(AddUserProfileDto profileDto)
        {
            var serviceResponse = await _service.AddProfileAsync(profileDto);

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
        public async Task<IActionResult> GetProfileById(int id)
        {
            var serviceResponse = await _service.GetProfileByIdAsync(id);

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
        public async Task<IActionResult> GetProfiles()
        {
            var serviceResponse = await _service.GetProfilesAsync();

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
        public async Task<IActionResult> UpdateProfile(int id, UpdateUserProfileDto profileDto)
        {
            var serviceResponse = await _service.UpdateProfileAsync(id, profileDto);

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
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var serviceResponse = await _service.DeleteProfileAsync(id);

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
