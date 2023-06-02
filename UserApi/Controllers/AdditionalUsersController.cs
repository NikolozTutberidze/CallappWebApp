using LogicLayer.AdditionalServices.Abstracts;
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
    public class AdditionalUsersController : ControllerBase
    {
        private readonly IAdditionalUserService _service;
        private readonly ILogger<AdditionalUsersController> _logger;

        public AdditionalUsersController(IAdditionalUserService service, ILogger<AdditionalUsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("userId/posts")]
        public async Task<IActionResult> GetPostsByUserId(int userId)
        {
            var serviceResponse = await _service.GetPostsByUserIdAsync(userId);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(serviceResponse.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

            return StatusCode((int)serviceResponse.StatusCode, serviceResponse.Message);
        }

        [HttpGet("userId/posts/postId")]
        public async Task<IActionResult> GetPostByUserId(int userId, int postId)
        {
            var serviceResponse = await _service.GetPostByUserIdAsync(userId, postId);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(serviceResponse.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

            return StatusCode((int)serviceResponse.StatusCode, serviceResponse.Message);
        }

        [HttpGet("userId/comments/commentId")]
        public async Task<IActionResult> GetCommentByUserId(int userId, int commentId)
        {
            var serviceResponse = await _service.GetCommentByUserIdAsync(userId, commentId);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(serviceResponse.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

            return StatusCode((int)serviceResponse.StatusCode, serviceResponse.Message);
        }

        [HttpGet("userId/albums")]
        public async Task<IActionResult> GetAlbumsByUserId(int userId)
        {
            var serviceResponse = await _service.GetAlbumsByUserIdAsync(userId);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(serviceResponse.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

            return StatusCode((int)serviceResponse.StatusCode, serviceResponse.Message);
        }

        [HttpGet("userId/albums/albumId")]
        public async Task<IActionResult> GetAlbumByUserid(int userId, int albumId)
        {
            var serviceResponse = await _service.GetAlbumByUserIdAsync(userId, albumId);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(serviceResponse.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

            return StatusCode((int)serviceResponse.StatusCode, serviceResponse.Message);
        }

        [HttpGet("userId/photos/photoId")]
        public async Task<IActionResult> GetPhotoByUserId(int userId, int photoId)
        {
            var serviceResponse = await _service.GetPhotoByUserIdAsync(userId, photoId);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(serviceResponse.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

            return StatusCode((int)serviceResponse.StatusCode, serviceResponse.Message);
        }

        [HttpGet("userId/todos")]
        public async Task<IActionResult> GetTodosByUserId(int userId)
        {
            var serviceResponse = await _service.GetTodosByUserIdAsync(userId);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(serviceResponse.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

            return StatusCode((int)serviceResponse.StatusCode, serviceResponse.Message);
        }

        [HttpGet("userId/todos/todoId")]
        public async Task<IActionResult> GetTodoByUserid(int userId, int todoId)
        {
            var serviceResponse = await _service.GetTodoByUserIdAsync(userId, todoId);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(serviceResponse.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + serviceResponse.StatusCode.ToString() + '\n');

            return StatusCode((int)serviceResponse.StatusCode, serviceResponse.Message);
        }
    }
}
