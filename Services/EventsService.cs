using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PuanConnect.Dtos.Event;
using PuanConnect.Interfaces;
using PuanConnect.Models;

namespace PuanConnect.Services;

public class EventsService : IEventsService
{
    private readonly IEventsRepository _eventsRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public EventsService(IEventsRepository eventsRepository, IMapper mapper, ICloudinaryService cloudinaryService, UserManager<User> userManager)
    {
        _eventsRepository = eventsRepository;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
        _userManager = userManager;
    }

    public async Task<Event> CreateEvent(EventFormDto createEventDto)
    {
        createEventDto.EventDate = createEventDto.EventDate.ToUniversalTime();
        createEventDto.CloseDate = createEventDto.CloseDate.ToUniversalTime();

        var thumbnailUrl = "https://cdn.discordapp.com/attachments/1201438198425980998/1213872327872348181/Puan-connect-bg.png?ex=65f70db3&is=65e498b3&hm=4f2f8d9c32553e83d009244e2ef71066d81d57924759be06c5539e56a6c8f311&";
        if (createEventDto.Thumbnail is not null)
        {
            thumbnailUrl = await _cloudinaryService.UploadImageAsync(createEventDto.Thumbnail);
        }

        // create event with Event Model
        var newEvent = new Event
        {
            Title = createEventDto.Title,
            Description = createEventDto.Description,
            LocationName = createEventDto.LocationName,
            LocationLat = createEventDto.LocationLat,
            LocationLng = createEventDto.LocationLng,
            Thumbnail = thumbnailUrl,
            EventDate = createEventDto.EventDate,
            CloseDate = createEventDto.CloseDate,
            Tags = createEventDto.Tags?.Split(","),
            MinReputation = createEventDto.MinReputation,
            MaxParticipants = createEventDto.MaxParticipants,
            Owner = createEventDto.Owner,
            CategoryId = Guid.Parse(createEventDto.CategoryId),
            CurrentParticipants = 0,
            ApprovedParticipants = 1,
        };

        var createdEvent = await _eventsRepository.CreateEvent(newEvent);

        await JoinEvent(createdEvent, createEventDto.Owner);

        return createdEvent;
    }

    public async Task<EventDto?> GetEventInfoById(Guid id)
    {
        var _event = await _eventsRepository.GetEventById(id)!;
        var result = _mapper.Map<Event, EventDto>(_event!);
        return result;
    }

    public async Task<Event?> GetEventById(Guid id)
    {
        var _event = await _eventsRepository.GetEventById(id);

        return _event;
    }

    public async Task<List<EventDto>> GetAllEvents()
    {
        var allEvent = await _eventsRepository.GetAllEvents();
        var result = _mapper.Map<List<Event>, List<EventDto>>(allEvent!);
        return result;
    }

    public async Task<List<YourEventsDto>> GetYourEvents(string userId)
    {
        var yourEvents = await _eventsRepository.GetYourEvents(userId);
        var result = _mapper.Map<List<Event>, List<YourEventsDto>>(yourEvents!);
        return result;
    }

    public async Task<List<GroupsDto>> GetGroups(string userId)
    {
        var groups = await _eventsRepository.GetGroups(userId);
        var result = _mapper.Map<List<Event>, List<GroupsDto>>(groups!);
        return result;
    }

    public async Task<Event?> UpdateEvent(EventFormDto updatedEvent)
    {
        var eventExist = await _eventsRepository.GetEventById(Guid.Parse(updatedEvent.Id.ToString()!))!;

        updatedEvent.EventDate = updatedEvent.EventDate.ToUniversalTime();
        updatedEvent.CloseDate = updatedEvent.CloseDate.ToUniversalTime();

        var thumbnailUrl = eventExist!.Thumbnail;
        if (updatedEvent.Thumbnail != null)
        {
            thumbnailUrl = await _cloudinaryService.UploadImageAsync(updatedEvent.Thumbnail);
        }

        var updatedEventModel = new Event
        {
            Id = eventExist!.Id,
            Title = updatedEvent.Title,
            Description = updatedEvent.Description,
            LocationName = updatedEvent.LocationName,
            LocationLat = updatedEvent.LocationLat,
            LocationLng = updatedEvent.LocationLng,
            Thumbnail = thumbnailUrl,
            EventDate = updatedEvent.EventDate,
            CloseDate = updatedEvent.CloseDate,
            Tags = updatedEvent.Tags?.Split(","),
            MinReputation = updatedEvent.MinReputation,
            MaxParticipants = updatedEvent.MaxParticipants,
            Owner = updatedEvent.Owner,
            CategoryId = Guid.Parse(updatedEvent.CategoryId),
            CurrentParticipants = eventExist!.CurrentParticipants,
            ApprovedParticipants = eventExist!.ApprovedParticipants,
            CreatedAt = eventExist!.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _eventsRepository.UpdateEvent(updatedEventModel);
        return result;
    }

    public async Task<Event?> JoinEvent(Event eventToJoin, User user)
    {
        eventToJoin.CurrentParticipants++;

        var isOwner = eventToJoin.Owner.Id.ToString() == user.Id;

        // check user is left from event
        var attendeeExist = await _eventsRepository.GetAttendeeByEventIdAndUserId(eventToJoin.Id, user.Id);

        if (attendeeExist != null)
        {
            if (attendeeExist.Status == AttendeeStatus.Left)
            {
                attendeeExist.Status = AttendeeStatus.Pending;

                var updateAttendee = await _eventsRepository.UpdateAttendee(attendeeExist);

                if (updateAttendee == null) return null;

                return eventToJoin;
            }
        }

        var attendee = new Attendee
        {
            EventId = eventToJoin.Id,
            User = user,
            IsApproved = isOwner,
            Event = eventToJoin,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Status = isOwner ? AttendeeStatus.Approved : AttendeeStatus.Pending,
        };

        var result = await _eventsRepository.JoinEvent(eventToJoin, attendee);

        return result;
    }

    public async Task<Event?> LeaveEvent(Event eventToLeave, User user)
    {
        eventToLeave.CurrentParticipants--;

        var attendee = await _eventsRepository.GetAttendeeByEventIdAndUserId(eventToLeave.Id, user.Id);

        if (attendee == null) return null;

        if (attendee.Status == AttendeeStatus.Approved)
        {
            var eventDate = eventToLeave.EventDate;
            var currentDate = DateTime.UtcNow;

            var timeDiff = (eventDate - currentDate).TotalHours;

            Console.WriteLine("Time Diff: " + timeDiff);

            var reputation = (float)(10 * (Math.Exp(-0.1 * timeDiff)));

            Console.WriteLine("Reputation: " + reputation);

            user.ReputationPoint -= reputation;

            Console.WriteLine("User Reputation: " + user.ReputationPoint);

            if (user.ReputationPoint < 0) user.ReputationPoint = 0;

            await _userManager.UpdateAsync(user);

            eventToLeave.ApprovedParticipants--;
        }

        var result = await _eventsRepository.LeaveEvent(eventToLeave, attendee);

        if (result == null) return null;

        return result;
    }

    public async Task<Event?> ApproveAttendee(Event eventToApprove, User user)
    {
        var attendee = await _eventsRepository.GetAttendeeByEventIdAndUserId(eventToApprove.Id, user.Id);

        if (attendee == null) return null;

        var result = await _eventsRepository.ApproveAttendee(eventToApprove, attendee);

        return result;
    }

    public async Task<Event?> DisapproveAttendee(Event eventToDisapprove, User user, User owner)
    {
        var eventDate = eventToDisapprove.EventDate;
        var currentDate = DateTime.UtcNow;

        var timeDiff = (eventDate - currentDate).TotalHours;

        Console.WriteLine("Time Diff: " + timeDiff);

        var reputation = (float)(10 * (Math.Exp(-0.1 * timeDiff)));

        Console.WriteLine("Reputation: " + reputation);

        owner.ReputationPoint -= reputation;

        Console.WriteLine("User Reputation: " + owner.ReputationPoint);

        if (owner.ReputationPoint < 0) owner.ReputationPoint = 0;

        await _userManager.UpdateAsync(owner);

        var attendee = await _eventsRepository.GetAttendeeByEventIdAndUserId(eventToDisapprove.Id, user.Id);

        if (attendee == null) return null;

        var result = await _eventsRepository.DisapproveAttendee(eventToDisapprove, attendee);

        return result;
    }

    public async Task<Event?> CloseEvent(Event eventToClose)
    {
        var result = await _eventsRepository.CloseEvent(eventToClose);

        return result;
    }

    public async Task<Attendee?> GetAttendeeByEventIdAndUserId(Guid eventId, string userId)
    {
        return await _eventsRepository.GetAttendeeByEventIdAndUserId(eventId,userId); 
    }
}