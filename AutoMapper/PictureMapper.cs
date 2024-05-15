using AutoMapper;
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
        }

    }
}
