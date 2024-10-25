using PuanConnect.Dtos.Event;
using PuanConnect.Models;

namespace PuanConnect.Interfaces;

public interface IEventsService
{
    Task<Event> CreateEvent(EventFormDto createEventDto);
    Task<EventDto?> GetEventInfoById(Guid id);
    Task<List<EventDto>> GetAllEvents();
    Task<List<YourEventsDto>> GetYourEvents(string userId);
    Task<List<GroupsDto>> GetGroups(string userId);
    Task<Event?> UpdateEvent(EventFormDto updatedEvent);
    Task<Event?> JoinEvent(Event eventToJoin, User user);
    Task<Event?> GetEventById(Guid id);
    Task<Event?> LeaveEvent(Event eventToLeave, User user);
    Task<Event?> ApproveAttendee(Event eventToApprove, User user);
    Task<Event?> DisapproveAttendee(Event eventToDisapprove, User user, User owner);
    Task<Event?> CloseEvent(Event eventToClose);
    Task<Attendee?> GetAttendeeByEventIdAndUserId(Guid eventId, string userId);
}