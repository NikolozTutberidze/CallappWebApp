using AutoMapper;
using DataAccessLayer.Models;
using LogicLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Mappings
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddUserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<AddUserProfileDto, UserProfile>();
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UpdateUserProfileDto, UserProfile>();
        }
    }
}
