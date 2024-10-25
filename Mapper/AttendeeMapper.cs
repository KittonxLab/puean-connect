using AutoMapper;
using PuanConnect.Dtos.Attendee;
using PuanConnect.Models;

namespace PuanConnect.Mapper;

public class AttendeeMapper : Profile
{
    public AttendeeMapper()
    {
        CreateMap<Attendee, EventAttendantDto>();
        CreateMap<Attendee, AttendancesDto>();
        CreateMap<User, AttendeeDto>();;
    }
}