using CaptureIt.DTOs.Bagde;
using CaptureIt.Models;
using AutoMapper;

namespace CaptureIt.AutoMapper
{
    public class BadgeMapper : Profile

    {
        public BadgeMapper()
        {
            CreateMap<BadgeRequest, Badge>();
            CreateMap<Badge, BadgeResponse>();
            CreateMap<BadgeUpdate, Badge>();
        }

    }
}
