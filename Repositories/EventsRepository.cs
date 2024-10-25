using Microsoft.EntityFrameworkCore;
using PuanConnect.Database;
using PuanConnect.Interfaces;
using PuanConnect.Models;

namespace PuanConnect.Repositories;

public class EventsRepository : IEventsRepository
{
  private readonly AppDBContext _dbContext;

  public EventsRepository(AppDBContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<Event> CreateEvent(Event createdEvent)
  {
    await _dbContext.Events.AddAsync(createdEvent);
    await _dbContext.SaveChangesAsync();
    return createdEvent;
  }

  public async Task<Event?> GetEventById(Guid id)
  {
    return await _GetEventQuery().FirstOrDefaultAsync(e => e.Id == id);
  }

  public async Task<List<Event>> GetAllEvents()
  {
    return await _GetEventQuery()
                                .Where(
                                  e => e.IsOpen
                                )
                                .ToListAsync();
  }

  private IQueryable<Event> _GetEventQuery()
  {
    var query = _dbContext.Events
                                .Include(e => e.Owner)
                                .Include(e => e.Category)
                                .Include(e => e.Attendees)
                                .ThenInclude(a => a.User);
    return query;
  }

  public async Task<List<Event>> GetYourEvents(string userId)
  {
    var ownedEvents = await _dbContext.Events
       .Include(e => e.Owner)
       .Where(e => e.Owner.Id == userId && e.EventDate > DateTime.UtcNow)
       .OrderByDescending(e => e.EventDate)
       .ToListAsync();


    return ownedEvents;
  }

  public async Task<List<Event>> GetGroups(string userId)
  {
    DateTime currentDate = DateTime.UtcNow;

    var attenedEvents = await _dbContext.Events
        .Include(e => e.Attendees)
        .ThenInclude(a => a.User)
        .Where(e => e.Attendees.Any(a => a.User.Id == userId && a.Status != AttendeeStatus.Left) && e.EventDate > currentDate && e.Owner.Id != userId)
        .OrderByDescending(e => e.EventDate)
        .ToListAsync();

    return attenedEvents;
  }

  public async Task<Event?> UpdateEvent(Event updatedEvent)
  {
    var eventExists = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id.ToString() == updatedEvent.Id.ToString());

    if (eventExists == null) return null;

    _dbContext.Entry(eventExists).CurrentValues.SetValues(updatedEvent);

    await _dbContext.SaveChangesAsync();
    return updatedEvent;
  }

  public async Task<Event?> JoinEvent(Event eventToJoin, Attendee attendee)
  {
    _dbContext.Attendees.Add(attendee);

    await _dbContext.SaveChangesAsync();

    return eventToJoin;
  }

  public async Task<Event?> LeaveEvent(Event eventToLeave, Attendee attendee)
  {
    attendee.IsApproved = false;
    attendee.Status = AttendeeStatus.Left;
    attendee.UpdatedAt = DateTime.UtcNow;

    _dbContext.Entry(attendee).State = EntityState.Modified;
    _dbContext.Entry(eventToLeave).State = EntityState.Modified;

    await _dbContext.SaveChangesAsync();

    return eventToLeave;
  }

  public async Task<Event?> ApproveAttendee(Event eventToApprove, Attendee attendee)
  {
    eventToApprove.ApprovedParticipants++;

    attendee.IsApproved = true;
    attendee.Status = AttendeeStatus.Approved;
    attendee.UpdatedAt = DateTime.UtcNow;

    _dbContext.Entry(attendee).State = EntityState.Modified;
    _dbContext.Entry(eventToApprove).State = EntityState.Modified;

    await _dbContext.SaveChangesAsync();

    return eventToApprove;
  }

  public async Task<Event?> DisapproveAttendee(Event eventToDisapprove, Attendee attendee)
  {
    eventToDisapprove.ApprovedParticipants--;

    attendee.IsApproved = false;
    attendee.Status = AttendeeStatus.Pending;
    attendee.UpdatedAt = DateTime.UtcNow;

    _dbContext.Entry(attendee).State = EntityState.Modified;
    _dbContext.Entry(eventToDisapprove).State = EntityState.Modified;

    await _dbContext.SaveChangesAsync();

    return eventToDisapprove;
  }

  public async Task<Attendee?> GetAttendeeByEventIdAndUserId(Guid eventId, string userId)
  {
    return await _dbContext.Attendees.FirstOrDefaultAsync(a => a.EventId == eventId && a.User.Id == userId);
  }

  public async Task<Attendee?> UpdateAttendee(Attendee attendee)
  {
    attendee.UpdatedAt = DateTime.UtcNow;

    var attendeeExists = await _dbContext.Attendees.FirstOrDefaultAsync(a => a.EventId == attendee.EventId && a.User.Id == attendee.User.Id);

    if (attendeeExists == null) return null;

    _dbContext.Entry(attendeeExists).CurrentValues.SetValues(attendee);

    await _dbContext.SaveChangesAsync();
    return attendee;
  }

  public async Task<Event?> CloseEvent(Event eventToClose)
  {
    eventToClose.IsOpen = false;
    eventToClose.UpdatedAt = DateTime.UtcNow;

    _dbContext.Entry(eventToClose).State = EntityState.Modified;

    await _dbContext.SaveChangesAsync();

    return eventToClose;
  }

  public async Task<int> CloseEventWorker()
  {
    await _dbContext.Events.Where(
      e => e.IsOpen && DateTime.Compare(e.CloseDate, DateTime.UtcNow) <= 0
    )
    .ExecuteUpdateAsync(
      s => s.SetProperty(
        e => e.IsOpen, false
      )
    );
    return 0;
  }
}