using LogicLayer.Dtos;
using LogicLayer.CustomResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Abstracts
{
    public interface IUserProfileService
    {
        Task<ServiceResponse<UserProfileDto>> AddProfileAsync(AddUserProfileDto profileDto);
        Task<ServiceResponse<UserProfileDto>> GetProfileByIdAsync(int id);
        Task<ServiceResponse<ICollection<UserProfileDto>>> GetProfilesAsync();
        Task<ServiceResponse<UserProfileDto>> UpdateProfileAsync(int id, UpdateUserProfileDto userDto);
        Task<ServiceResponse<UserProfileDto>> DeleteProfileAsync(int id);
    }
}
