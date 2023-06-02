using LogicLayer.AdditionalDtos;
using LogicLayer.CustomResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.AdditionalServices.Abstracts
{
    public interface IAdditionalUserService
    {
        Task<ServiceResponse<ICollection<PostDto>>> GetPostsByUserIdAsync(int userId);
        Task<ServiceResponse<PostDto>> GetPostByUserIdAsync(int userId, int postId);
        Task<ServiceResponse<CommentDto>> GetCommentByUserIdAsync(int userId, int commentId);
        Task<ServiceResponse<ICollection<AlbumDto>>> GetAlbumsByUserIdAsync(int userId);
        Task<ServiceResponse<AlbumDto>> GetAlbumByUserIdAsync(int userId, int albumId);
        Task<ServiceResponse<PhotoDto>> GetPhotoByUserIdAsync(int userId, int photoId);
        Task<ServiceResponse<ICollection<TodoDto>>> GetTodosByUserIdAsync(int userId);
        Task<ServiceResponse<TodoDto>> GetTodoByUserIdAsync(int userId, int todoId);
    }
}
