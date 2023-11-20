using AutoMapper;
using Shopping.Entity.Dto;
using Shopping.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.AutoMapper.AutoMapperProFiles
{
    public class UserProFile : Profile
    {
        public UserProFile()
        {
            CreateMap<User,LoginDto>();
            CreateMap<RegisterDto, User>();
        }
    }
}
