using AutoMapper;
using CaptureIt.DTOs.Like;
using CaptureIt.Models;

namespace CaptureIt.AutoMapper
{
    public class LikeMapper : Profile
    {
        public LikeMapper() 
        {
            CreateMap<LikeRequest, Like>();
            CreateMap<Like, LikeResponse>();
        }
    }
}
