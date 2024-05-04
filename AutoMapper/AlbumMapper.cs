using CaptureIt.DTOs.Album;
using CaptureIt.Models;
using AutoMapper;

namespace CaptureIt.AutoMapper
{
    public class AlbumMapper : Profile
    {
        public AlbumMapper()
        {
            CreateMap<AlbumRequest, Album>();
            CreateMap<Album, AlbumResponse>();
        }
    }
}
