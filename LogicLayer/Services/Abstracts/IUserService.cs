using DataAccessLayer.Models;
using LogicLayer.Dtos;
using LogicLayer.CustomResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Abstracts
{
    public interface IUserService
    {
        Task<ServiceResponse<UserDto>> AddUserAsync(AddUserDto userDto);
        Task<ServiceResponse<UserDto>> GetUserByIdAsync(int id);
        Task<ServiceResponse<ICollection<UserDto>>> GetUsersAsync();
        Task<ServiceResponse<UserDto>> UpdateUserAsync(int id, UpdateUserDto userDto);
        Task<ServiceResponse<UserDto>> DeleteUserAsync(int id);
        Task<ServiceResponse<string>> LoginAsync(LoginDto request);
        Task<ServiceResponse<UserDto>> RegisterAsync(AddUserDto userDto);
    }
}
