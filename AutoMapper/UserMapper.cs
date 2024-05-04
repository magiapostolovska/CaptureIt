using AutoMapper;
using CaptureIt.Authentication;
using CaptureIt.DTOs.User;
using CaptureIt.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CaptureIt.AutoMapper
{
    public class UserMapper : Profile
    {
        public UserMapper()     
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<RegisterModel, User>();



        }
    }
}
