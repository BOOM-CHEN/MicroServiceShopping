using AutoMapper;
using ShoppingUser.EntityModel.Dto;
using ShoppingUser.EntityModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.EntityModel.AutoMapper.AutoMapperProFiles
{
    public class UserProFile : Profile
    {
        public UserProFile()
        {
            CreateMap<AddUserDto, User>();
            CreateMap<User, UserInfoDto>();
        }
    }
}
