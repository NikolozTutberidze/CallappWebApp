using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Abstracts;
using LogicLayer.Dtos;
using LogicLayer.Mappings;
using LogicLayer.CustomResponse;
using LogicLayer.Services.Abstracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Imlementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repository, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;

            _mapper = mapper;
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = new Mapper(config);

            _configuration = configuration;
        }
        public async Task<ServiceResponse<UserDto>> AddUserAsync(AddUserDto userDto)
        {
            var result = new ServiceResponse<UserDto>();

            if ((await _repository.GetUsersAsync()).Any(u => u.Username == userDto.Username && u.Password == userDto.Password))
            {
                result.StatusCode = HttpStatusCode.Conflict;
                result.Message = "User exists";

                return result;
            }

            if (!Validation(userDto))
            {
                result.StatusCode = HttpStatusCode.BadRequest;
                result.Message = "Input is incorrect";

                return result;
            }

            var user = _mapper.Map<User>(userDto);
            await _repository.AddUserAsync(user);

            result.StatusCode = HttpStatusCode.Created;
            result.Data = _mapper.Map<UserDto>(user);

            return result;
        }

        private bool Validation(AddUserDto userDto)
        {
            if (userDto.Username.Any(l => char.IsLetterOrDigit(l) == false))
                return false;
            if (userDto.Password.Any(l => char.IsLetterOrDigit(l) == false))
                return false;

            return true;
        }

        public async Task<ServiceResponse<UserDto>> GetUserByIdAsync(int id)
        {
            var result = new ServiceResponse<UserDto>();

            var user = await _repository.GetUserByIdAsync(id);
            if (user == null)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "User not found";

                return result;
            }

            user.Profile = await _repository.GetProfileAsync(user.Id);

            var userDto = _mapper.Map<UserDto>(user);

            result.Data = userDto;
            result.StatusCode = HttpStatusCode.OK;

            return result;
        }

        public async Task<ServiceResponse<ICollection<UserDto>>> GetUsersAsync()
        {
            var result = new ServiceResponse<ICollection<UserDto>>();

            var users = await _repository.GetUsersAsync();

            if (users.Count == 0)
            {
                result.StatusCode = HttpStatusCode.NoContent;
                result.Message = "Users empty";

                return result;
            }

            foreach (var user in users)
            {
                user.Profile = await _repository.GetProfileAsync(user.Id);
            }

            var userDtos = users.Select(u => _mapper.Map<UserDto>(u)).ToList();

            result.Data = userDtos;
            result.StatusCode = HttpStatusCode.OK;

            return result;
        }

        public async Task<ServiceResponse<UserDto>> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            var result = new ServiceResponse<UserDto>();

            var user = await _repository.GetUserByIdAsync(id);

            if (user == null)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "Given id is incorrect";

                return result;
            }

            if (!Validation(userDto))
            {
                result.StatusCode = HttpStatusCode.BadRequest;
                result.Message = "Input is incorrect";

                return result;
            }

            user = _mapper.Map<User>(userDto);
            user.Id = id;

            await _repository.UpdateUserAsync(user);

            var updatedUser = _mapper.Map<UserDto>(user);

            result.StatusCode = HttpStatusCode.OK;
            result.Data = updatedUser;

            return result;
        }

        private bool Validation(UpdateUserDto userDto)
        {
            if (userDto.Username.Any(l => char.IsLetterOrDigit(l) == false))
                return false;
            if (userDto.Password.Any(l => char.IsLetterOrDigit(l) == false))
                return false;

            return true;
        }

        public async Task<ServiceResponse<UserDto>> DeleteUserAsync(int id)
        {
            var result = new ServiceResponse<UserDto>();

            var user = await _repository.GetUserByIdAsync(id);

            if (user == null)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = "Given id is incorrect";

                return result;
            }

            await _repository.DeleteUserAsync(id);

            result.StatusCode = HttpStatusCode.NoContent;

            return result;
        }

        public async Task<ServiceResponse<string>> LoginAsync(LoginDto request)
        {
            var result = new ServiceResponse<string>();

            if (!(await _repository.GetUsersAsync()).Any(u => u.Username == request.Username && u.Password == request.Password))
            {
                result.StatusCode = HttpStatusCode.NotFound;

                return result;
            }

            var user = (await _repository.GetUsersAsync()).FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        _configuration.GetSection("Jwt:Key").Value));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string jwtToken = tokenHandler.WriteToken(token);

            result.StatusCode = HttpStatusCode.OK;
            result.Data = jwtToken;

            return result;
        }

        public async Task<ServiceResponse<UserDto>> RegisterAsync(AddUserDto userDto)
        {
            var result = new ServiceResponse<UserDto>();

            if ((await _repository.GetUsersAsync()).Any(u => u.Username == userDto.Username))
            {
                result.StatusCode = HttpStatusCode.Conflict;
                result.Message = "User exists";

                return result;
            }

            if (!Validation(userDto))
            {
                result.StatusCode = HttpStatusCode.BadRequest;
                result.Message = "Input is incorrect";

                return result;
            }

            var user = _mapper.Map<User>(userDto);
            await _repository.AddUserAsync(user);

            result.StatusCode = HttpStatusCode.Created;
            result.Data = _mapper.Map<UserDto>(user);

            return result;
        }
    }
}
