using System.Text.RegularExpressions;
using AutoMapper;
using PuanConnect.Dtos.Event;
using PuanConnect.Models;

namespace PuanConnect.Mapper;

public class EventMapper : Profile
{
    public EventMapper()
    {
        CreateMap<EventDto, Event>().ReverseMap();
        CreateMap<EventDto, Event>().ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner));
        CreateMap<Event, HeldEventsDto>();
        CreateMap<Event, AttendEventsDto>();
        CreateMap<Event, YourEventsDto>();
        CreateMap<Event, GroupsDto>();
        CreateMap<EventFormDto, Event>();
    }
}