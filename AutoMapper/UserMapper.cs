﻿using AutoMapper;
using CaptureIt.Authentication;
using CaptureIt.DTOs.PasswordRecovery;
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
            CreateMap<UserUpdate, User>();
            CreateMap<NewPassword, User>();
            CreateMap<User, UserDetails>();
            CreateMap<User,EventParticipant>();



        }
    }
}
