using LogicLayer.AdditionalDtos;
using LogicLayer.AdditionalServices.Abstracts;
using LogicLayer.CustomResponse;
using System.Net.Http.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Dtos;
using System.Net;
using DataAccessLayer.Repositories.Abstracts;
using System.ComponentModel.Design;

namespace LogicLayer.AdditionalServices.Implementations
{
    public class AdditionalUserService : IAdditionalUserService
    {
        private readonly HttpClient _client;
        private readonly IUserRepository _userRepository;

        public AdditionalUserService(IUserRepository userRepository)
        {
            _client = new HttpClient();
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<ICollection<PostDto>>> GetPostsByUserIdAsync(int userId)
        {
            var result = new ServiceResponse<ICollection<PostDto>>();

            var userExists = (await _userRepository.GetUsersAsync()).Any(u => u.Id == userId);
            if (!userExists)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User not found";

                return result;
            }

            HttpResponseMessage responsePosts = await _client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}/posts");

            if (responsePosts.IsSuccessStatusCode)
            {
                var posts = await responsePosts.Content.ReadAsAsync<List<PostDto>>();

                HttpResponseMessage responseComments = new HttpResponseMessage();

                var comments = new List<CommentDto>();

                foreach (var post in posts)
                {
                    responseComments = await _client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{post.Id}/comments");
                    if (responseComments.IsSuccessStatusCode)
                    {
                        comments = await responseComments.Content.ReadAsAsync<List<CommentDto>>();

                        if (post.Comments == null)
                            post.Comments = new List<CommentDto>();

                        post.Comments.AddRange(comments);
                    }
                    else
                    {
                        result.StatusCode = responseComments.StatusCode;
                        result.Message = "Problem during retrieving comments";

                        return result;
                    }
                }

                result.StatusCode = responsePosts.StatusCode;
                result.Data = posts;

                return result;
            }

            result.StatusCode = responsePosts.StatusCode;
            result.Message = " Problem during retrieving posts";

            return result;
        }

        public async Task<ServiceResponse<PostDto>> GetPostByUserIdAsync(int userId, int postId)
        {
            var result = new ServiceResponse<PostDto>();

            var userExists = (await _userRepository.GetUsersAsync()).Any(u => u.Id == userId);
            if (!userExists)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User not found";

                return result;
            }

            HttpResponseMessage responsePosts = await _client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}/posts");

            if (responsePosts.IsSuccessStatusCode)
            {
                var posts = await responsePosts.Content.ReadAsAsync<List<PostDto>>();

                var post = posts.Where(p => p.Id == postId).FirstOrDefault();

                if (post == null)
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "Post not found";

                    return result;
                }

                HttpResponseMessage responseComments = await _client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{post.Id}/comments");

                if (responseComments.IsSuccessStatusCode)
                {
                    var comments = await responseComments.Content.ReadAsAsync<List<CommentDto>>();

                    post.Comments = comments;
                }
                else
                {
                    result.StatusCode = responseComments.StatusCode;
                    result.Message = "Problem during retrieving comments";

                    return result;
                }

                result.StatusCode = responsePosts.StatusCode;
                result.Data = post;

                return result;
            }

            result.StatusCode = responsePosts.StatusCode;
            result.Message = "Problem during retrieving post";

            return result;
        }

        public async Task<ServiceResponse<CommentDto>> GetCommentByUserIdAsync(int userId, int commentId)
        {
            var result = new ServiceResponse<CommentDto>();

            var userExists = (await _userRepository.GetUsersAsync()).Any(u => u.Id == userId);
            if (!userExists)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User not found";

                return result;
            }

            HttpResponseMessage responsePosts = await _client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}/posts");

            if (responsePosts.IsSuccessStatusCode)
            {
                var posts = await responsePosts.Content.ReadAsAsync<List<PostDto>>();

                HttpResponseMessage responseComments = new HttpResponseMessage();

                foreach (var post in posts)
                {
                    responseComments = await _client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{post.Id}/comments");

                    if (responseComments.IsSuccessStatusCode)
                    {
                        var comments = await responseComments.Content.ReadAsAsync<List<CommentDto>>();

                        foreach (var comment in comments)
                        {
                            if (comment.Id == commentId)
                            {
                                result.StatusCode = responseComments.StatusCode;
                                result.Data = comment;

                                return result;
                            }
                        }
                    }

                    else
                    {
                        result.StatusCode = responseComments.StatusCode;
                        result.Message = "Problem during retrieving comments";

                        return result;
                    }
                }
            }

            result.StatusCode = responsePosts.StatusCode;
            result.Message = "Problem during retrieving posts";

            return result;
        }

        public async Task<ServiceResponse<ICollection<AlbumDto>>> GetAlbumsByUserIdAsync(int userId)
        {
            var result = new ServiceResponse<ICollection<AlbumDto>>();

            var userExists = (await _userRepository.GetUsersAsync()).Any(u => u.Id == userId);
            if (!userExists)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User not found";

                return result;
            }

            HttpResponseMessage responseAlbums = await _client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}/albums");

            if (responseAlbums.IsSuccessStatusCode)
            {
                var albums = await responseAlbums.Content.ReadAsAsync<List<AlbumDto>>();

                HttpResponseMessage responsePhotos = new HttpResponseMessage();

                var photos = new List<PhotoDto>();

                foreach (var album in albums)
                {
                    responsePhotos = await _client.GetAsync($"https://jsonplaceholder.typicode.com/albums/{album.Id}/photos");
                    if (responsePhotos.IsSuccessStatusCode)
                    {
                        photos = await responsePhotos.Content.ReadAsAsync<List<PhotoDto>>();

                        if (album.Photos == null)
                            album.Photos = new List<PhotoDto>();

                        album.Photos.AddRange(photos);
                    }
                    else
                    {
                        result.StatusCode = responsePhotos.StatusCode;
                        result.Message = "Problem during retrieving photos";

                        return result;
                    }
                }

                result.StatusCode = responseAlbums.StatusCode;
                result.Data = albums;

                return result;
            }

            result.StatusCode = responseAlbums.StatusCode;
            result.Message = " Problem during retrieving albums";

            return result;
        }

        public async Task<ServiceResponse<AlbumDto>> GetAlbumByUserIdAsync(int userId, int albumId)
        {
            var result = new ServiceResponse<AlbumDto>();

            var userExists = (await _userRepository.GetUsersAsync()).Any(u => u.Id == userId);
            if (!userExists)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User not found";

                return result;
            }

            HttpResponseMessage responseAlbums = await _client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}/albums");

            if (responseAlbums.IsSuccessStatusCode)
            {
                var albums = await responseAlbums.Content.ReadAsAsync<List<AlbumDto>>();

                var album = albums.Where(a => a.Id == albumId).FirstOrDefault();

                if (album == null)
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "Album not found";

                    return result;
                }

                HttpResponseMessage responsePhotos = await _client.GetAsync($"https://jsonplaceholder.typicode.com/albums/{album.Id}/photos");

                if (responsePhotos.IsSuccessStatusCode)
                {
                    var photos = await responsePhotos.Content.ReadAsAsync<List<PhotoDto>>();

                    album.Photos = photos;
                }
                else
                {
                    result.StatusCode = responsePhotos.StatusCode;
                    result.Message = "Problem during retrieving photos";

                    return result;
                }

                result.StatusCode = responseAlbums.StatusCode;
                result.Data = album;

                return result;
            }

            result.StatusCode = responseAlbums.StatusCode;
            result.Message = "Problem during retrieving album";

            return result;
        }

        public async Task<ServiceResponse<PhotoDto>> GetPhotoByUserIdAsync(int userId, int photoId)
        {
            var result = new ServiceResponse<PhotoDto>();

            var userExists = (await _userRepository.GetUsersAsync()).Any(u => u.Id == userId);
            if (!userExists)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User not found";

                return result;
            }

            HttpResponseMessage responseAlbums = await _client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}/albums");

            if (responseAlbums.IsSuccessStatusCode)
            {
                var albums = await responseAlbums.Content.ReadAsAsync<List<AlbumDto>>();

                HttpResponseMessage responsePhotos = new HttpResponseMessage();

                foreach (var album in albums)
                {
                    responsePhotos = await _client.GetAsync($"https://jsonplaceholder.typicode.com/albums/{album.Id}/photos");

                    if (responsePhotos.IsSuccessStatusCode)
                    {
                        var photos = await responsePhotos.Content.ReadAsAsync<List<PhotoDto>>();

                        foreach (var photo in photos)
                        {
                            if (photo.Id == photoId)
                            {
                                result.StatusCode = responsePhotos.StatusCode;
                                result.Data = photo;

                                return result;
                            }
                        }
                    }

                    else
                    {
                        result.StatusCode = responsePhotos.StatusCode;
                        result.Message = "Problem during retrieving photos";

                        return result;
                    }
                }
            }

            result.StatusCode = responseAlbums.StatusCode;
            result.Message = "Problem during retrieving albums";

            return result;
        }

        public async Task<ServiceResponse<ICollection<TodoDto>>> GetTodosByUserIdAsync(int userId)
        {
            var result = new ServiceResponse<ICollection<TodoDto>>();

            var userExists = (await _userRepository.GetUsersAsync()).Any(u => u.Id == userId);
            if (!userExists)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User not found";

                return result;
            }

            HttpResponseMessage response = await _client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}/todos");

            if (response.IsSuccessStatusCode)
            {
                var todos = await response.Content.ReadAsAsync<List<TodoDto>>();

                result.StatusCode = response.StatusCode;
                result.Data = todos;

                return result;
            }

            result.StatusCode = response.StatusCode;
            result.Message = " Something went wrong during retrieving todos";

            return result;
        }

        public async Task<ServiceResponse<TodoDto>> GetTodoByUserIdAsync(int userId, int todoId)
        {
            var result = new ServiceResponse<TodoDto>();

            var userExists = (await _userRepository.GetUsersAsync()).Any(u => u.Id == userId);
            if (!userExists)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User not found";

                return result;
            }

            HttpResponseMessage responseTodos = await _client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}/todos");

            if (responseTodos.IsSuccessStatusCode)
            {
                var todos = await responseTodos.Content.ReadAsAsync<List<TodoDto>>();

                var todo = todos.Where(t => t.Id == todoId).FirstOrDefault();

                if (todo == null)
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "Todo not found";

                    return result;
                }

                result.StatusCode = responseTodos.StatusCode;
                result.Data = todo;

                return result;
            }

            result.StatusCode = responseTodos.StatusCode;
            result.Message = "Problem during retrieving todos";

            return result;
        }
    }
}
