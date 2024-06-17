﻿using AutoMapper;
using CaptureIt.DTOs.Picture;
using CaptureIt.Models;

namespace CaptureIt.AutoMapper
{
    public class PictureMapper : Profile
    {
        public PictureMapper() 
        {
            CreateMap<PictureRequest, Picture>();
            CreateMap<Picture, PictureResponse>();
            CreateMap<PictureUpdate,  Picture>();
            CreateMap<PictureLikes, Picture>();
            CreateMap<PictureComments, Picture>();
            CreateMap<PictureAuthor, Picture>();    
            CreateMap<Picture, PictureUrl>();
        }

    }
}
