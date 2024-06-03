using AutoMapper;
using CaptureIt.DTOs.Event;
using CaptureIt.Models;

namespace CaptureIt.AutoMapper
{
    public class EventMapper : Profile
    {
        public EventMapper()    
        {
            CreateMap<EventRequest, Event>();
            CreateMap<Event, EventResponse>();
            CreateMap<EventUpdate, Event>();
            CreateMap<EventParticipantRequest, Event>()
                 .ForMember(dest => dest.Participants, opt => opt.Ignore());
            CreateMap<Event, EventParticipantResponse>();
            CreateMap<Event, EventParticipantList>();
            CreateMap<EventOwner, Event>();
            CreateMap<Event, EventDetails>();
        }    
    }
}
