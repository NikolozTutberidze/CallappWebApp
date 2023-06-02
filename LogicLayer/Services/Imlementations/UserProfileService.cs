using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Abstracts;
using LogicLayer.Dtos;
using LogicLayer.Mappings;
using LogicLayer.CustomResponse;
using LogicLayer.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Imlementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _repository;
        private readonly IMapper _mapper;

        public UserProfileService(IUserProfileRepository repository, IMapper mapper)
        {
            _repository = repository;

            _mapper = mapper;
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = new Mapper(config);
        }
        public async Task<ServiceResponse<UserProfileDto>> AddProfileAsync(AddUserProfileDto profileDto)
        {
            var result = new ServiceResponse<UserProfileDto>();

            if ((await _repository.GetProfilesAsync()).Any(p => p.PersonalNumber == profileDto.PersonalNumber))
            {
                result.StatusCode = HttpStatusCode.Conflict;
                result.Message = "Profile exists";

                return result;
            }

            if (!Validation(profileDto))
            {
                result.StatusCode = HttpStatusCode.BadRequest;
                result.Message = "Input is incorrect";

                return result;
            }

            if (!(await _repository.UserExistsAsync(profileDto.UserId)))
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User with given Id not found";

                return result;
            }

            if ((await _repository.GetProfilesAsync()).Any(p => p.UserId == profileDto.UserId))
            {
                result.StatusCode = HttpStatusCode.BadRequest;
                result.Message = "User with given Id is connected to other Profile";

                return result;
            }

            var profile = _mapper.Map<UserProfile>(profileDto);
            profile.UserId = profileDto.UserId;
            profile.User = await _repository.GetUserByIdAsync(profileDto.UserId);

            _repository.UpdateUser(profile.User);

            await _repository.AddProfileAsync(profile);

            result.StatusCode = HttpStatusCode.Created;
            result.Data = _mapper.Map<UserProfileDto>(profile);

            return result;
        }

        private bool Validation(AddUserProfileDto profileDto)
        {
            if (profileDto.Firstname.Any(l => char.IsLetter(l) == false))
                return false;
            if (profileDto.Lastname.Any(l => char.IsLetter(l) == false))
                return false;
            if (profileDto.PersonalNumber.Any(l => char.IsDigit(l) == false))
                return false;

            return true;
        }

        public async Task<ServiceResponse<UserProfileDto>> GetProfileByIdAsync(int id)
        {
            var result = new ServiceResponse<UserProfileDto>();

            var profile = await _repository.GetProfileByIdAsync(id);
            if (profile == null)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "Profile not found";

                return result;
            }

            profile.User = await _repository.GetUserByIdAsync(profile.UserId);

            var profileDto = _mapper.Map<UserProfileDto>(profile);

            result.Data = profileDto;
            result.StatusCode = HttpStatusCode.OK;

            return result;
        }

        public async Task<ServiceResponse<ICollection<UserProfileDto>>> GetProfilesAsync()
        {
            var result = new ServiceResponse<ICollection<UserProfileDto>>();

            var profiles = await _repository.GetProfilesAsync();

            if (profiles.Count == 0)
            {
                result.StatusCode = HttpStatusCode.NoContent;
                result.Message = "Profiles empty";

                return result;
            }

            foreach (var profile in profiles)
            {
                profile.User = await _repository.GetUserByIdAsync(profile.UserId);
            }

            var profileDtos = profiles.Select(p => _mapper.Map<UserProfileDto>(p)).ToList();

            result.Data = profileDtos;
            result.StatusCode = HttpStatusCode.OK;

            return result;
        }

        public async Task<ServiceResponse<UserProfileDto>> UpdateProfileAsync(int id, UpdateUserProfileDto profileDto)
        {
            var result = new ServiceResponse<UserProfileDto>();

            var profile = await _repository.GetProfileByIdAsync(id);

            if (profile == null)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "Given id is incorrect";

                return result;
            }

            if (!Validation(profileDto))
            {
                result.StatusCode = HttpStatusCode.BadRequest;
                result.Message = "Input is incorrect";

                return result;
            }

            var userId = profile.UserId;

            profile = _mapper.Map<UserProfile>(profileDto);
            profile.Id = id;
            profile.UserId = userId;

            await _repository.UpdateProfileAsync(profile);

            var updatedProfile = _mapper.Map<UserProfileDto>(profile);

            result.StatusCode = HttpStatusCode.OK;
            result.Data = updatedProfile;

            return result;
        }

        private bool Validation(UpdateUserProfileDto profileDto)
        {
            if (profileDto.Firstname.Any(l => char.IsLetter(l) == false))
                return false;
            if (profileDto.Lastname.Any(l => char.IsLetter(l) == false))
                return false;
            if (profileDto.PersonalNumber.Any(l => char.IsDigit(l) == false))
                return false;

            return true;
        }

        public async Task<ServiceResponse<UserProfileDto>> DeleteProfileAsync(int id)
        {
            var result = new ServiceResponse<UserProfileDto>();

            var profile = await _repository.GetProfileByIdAsync(id);

            if (profile == null)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "Given id is incorrect";

                return result;
            }

            await _repository.DeleteProfileAsync(id);

            result.StatusCode = HttpStatusCode.NoContent;

            return result;
        }
    }
}
