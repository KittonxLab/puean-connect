using PuanConnect.Dtos.Event;
using PuanConnect.Models;

namespace PuanConnect.Interfaces;

public interface IEventsRepository
{
    Task<Event> CreateEvent(Event createdEvent);
    Task<Event?> GetEventById(Guid id);
    Task<List<Event>> GetAllEvents();
    Task<List<Event>> GetYourEvents(string userId);
    Task<List<Event>> GetGroups(string userId);
    Task<Event?> UpdateEvent(Event updatedEvent);
    Task<Event?> JoinEvent(Event eventToJoin, Attendee attendee);
    Task<Event?> LeaveEvent(Event eventToLeave, Attendee attendee);
    Task<Attendee?> GetAttendeeByEventIdAndUserId(Guid eventId, string userId);
    Task<Attendee?> UpdateAttendee(Attendee attendee);
    Task<Event?> ApproveAttendee(Event eventToApprove, Attendee attendee);
    Task<Event?> DisapproveAttendee(Event eventToDisapprove, Attendee attendee);
    Task<Event?> CloseEvent(Event eventToClose);
    
    Task<int> CloseEventWorker();
}