using AutoMapper;
using CaptureIt.DTOs.Event;
using CaptureIt.DTOs.Picture;
using CaptureIt.Models;
using System.Linq;

namespace CaptureIt.AutoMapper
{
    public class EventMapper : Profile
    {
        public EventMapper()
        {
            CreateMap<EventRequest, Event>();
            CreateMap<Event, EventResponse>()
                  .AfterMap((src, dest, context) =>
                  {
                      var pictures = src.Albums.SelectMany(a => a.Pictures).ToList();
                      dest.Pictures = context.Mapper.Map<List<PictureUrl>>(pictures);
                  });
            CreateMap<EventUpdate, Event>();
            CreateMap<EventParticipantRequest, Event>()
                 .ForMember(dest => dest.Participants, opt => opt.Ignore());
            CreateMap<Event, EventParticipantResponse>();
            

            CreateMap<Event, EventParticipantList>();
            CreateMap<EventOwner, Event>();
            CreateMap<Event, EventDetails>();

            new PictureMapper();
        }
    }
}
