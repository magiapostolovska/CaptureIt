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
        }    
    }
}
