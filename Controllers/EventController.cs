using Microsoft.AspNetCore.Mvc;
using PuanConnect.Dtos.Event;
using PuanConnect.Interfaces;
using PuanConnect.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace PuanConnect.Controllers;

public class EventController : Controller
{
    private readonly ILogger<EventController> _logger;
    private readonly IMapper _mapper;
    private readonly IEventsService _eventsService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUsersService _usersService;

    private readonly ICategoryService _categoryService;
    private readonly IEmailService _emailService;


    public EventController(
        ILogger<EventController> logger,
        IMapper mapper,
        IEventsService eventsService,
        ICurrentUserService currentUserService,
        IUsersService usersService,
        ICategoryService categoryService,
        IEmailService emailService
    )
    {
        _logger = logger;
        _eventsService = eventsService;
        _currentUserService = currentUserService;
        _usersService = usersService;
        _categoryService = categoryService;
        _mapper = mapper;
        _emailService = emailService;
    }


    public async Task<ActionResult> Index([FromQuery(Name = "id")] string id)
    {
        try
        {
            var viewModel = new EventViewModel();
            var eventId = Guid.Parse(id);
            var userId = await _currentUserService.GetCurrentUserId();

            if (userId is not null)
            {
                var user = await _usersService.GetUserById(userId.ToString()!);
                viewModel.User = user;
            }

            var _event = await _eventsService.GetEventInfoById(eventId);
            if (_event is null) return RedirectToAction("Index", "Home");
            viewModel.Event = _event;

            return View(viewModel);
        }
        catch
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<ActionResult> Create()
    {
        var userId = await _currentUserService.GetCurrentUserId();
        if (userId is null) return RedirectToAction("Login", "Account");

        var user = await _usersService.GetUserById(userId.ToString()!);
        var categories = await _categoryService.GetAllCategories();

        var newEvent = new EventFormDto
        {
            Owner = user!,
            CategoriesList = categories,
        };

        Console.WriteLine("User: " + user!.Id);

        ViewBag.Action = "Create";
        ViewBag.submitText = "Create";
        ViewBag.RedirectCtrl = "Home";
        return View("~/Views/Event/Form.cshtml", newEvent);
    }

    [HttpPost]
    public async Task<ActionResult> Create(EventFormDto newEvent)
    {
        var categories = await _categoryService.GetAllCategories();
        newEvent.CategoriesList = categories;

        var userId = await _currentUserService.GetCurrentUserId();
        if (userId is null) return RedirectToAction("Login", "Account");

        var user = await _usersService.GetUserById(userId.ToString()!);
        newEvent.Owner = user!;

        // Remove ModelState for Owner and CategoriesList
        ModelState.Remove("Owner");
        ModelState.Remove("CategoriesList");
        ViewBag.Action = "Create";
        ViewBag.submitText = "Create";
        ViewBag.RedirectCtrl = "Home";

        ViewBag.Action = "Create";
        if (!ModelState.IsValid) return View("~/Views/Event/Form.cshtml", newEvent);

        // check if close date must at lest 2 days before event date
        if (newEvent.CloseDate > newEvent.EventDate.AddDays(-2))
        {
            TempData["Error"] = "Close date must be at least 2 days before the event date";
            return View("~/Views/Event/Form.cshtml", newEvent);
        }

        // check minReputation is 0 - 100
        if (newEvent.MinReputation < 0 || newEvent.MinReputation > 100)
        {
            TempData["Error"] = "Min reputation must be between 0 and 100";
            return View("~/Views/Event/Form.cshtml", newEvent);
        }

        // check maxParticipants is less than 1
        if (newEvent.MaxParticipants < 1)
        {
            TempData["Error"] = "Max participants must be more than or equal 1";
            return View("~/Views/Event/Form.cshtml", newEvent);
        }

        var result = await _eventsService.CreateEvent(newEvent);

        if (result is null)
        {
            TempData["Error"] = "An error occurred while creating the event";
            return View("~/Views/Event/Form.cshtml", newEvent);
        }

        string message = $"Dear {user!.Firstname} {user.Lastname},\n\n" +
                         $"You've successfully created an event titled \"{result.Title}\"." +
                         $"\n\nEvent Details:" +
                         $"\n- Description: {result.Description}" +
                         $"\n- Date: {result.EventDate.ToString("F")}" +
                         $"\n- Close Date: {result.CloseDate.ToString("F")}" +
                         $"\n- Location: {result.LocationName}" +
                         $"\n- Minimum Reputation Required: {result.MinReputation}" +
                         $"\n- Maximum Participants: {result.MaxParticipants}";
        await _emailService.SendCustomSubject(user.Email!, message, "Event Create Confirmation");
        Console.WriteLine("Email sent!");

        return RedirectToAction("Index", "Event", new { id = result.Id });
    }

    [Authorize]
    public async Task<ActionResult> Edit([FromQuery(Name = "id")] string id)
    {
        var eventId = Guid.Parse(id);
        var userId = await _currentUserService.GetCurrentUserId();

        if (userId is null) return RedirectToAction("Index", "Home");

        var user = await _usersService.GetUserById(userId.ToString()!);
        var _event = await _eventsService.GetEventInfoById(eventId);

        if (_event is null || !_event.IsOpen || user?.Id != _event.Owner.Id) return RedirectToAction("Index", "Home");

        var categories = await _categoryService.GetAllCategories();

        var editEvent = new EventFormDto
        {
            Id = eventId,
            Title = _event.Title,
            Description = _event.Description,
            LocationName = _event.LocationName,
            LocationLat = _event.LocationLat,
            LocationLng = _event.LocationLng,
            EventDate = _event.EventDate,
            CloseDate = _event.CloseDate,
            Tags = string.Join(",", _event.Tags!),
            MinReputation = _event.MinReputation,
            MaxParticipants = _event.MaxParticipants,
            CategoryId = _event.Category.Id.ToString(),
            Owner = user!,
            CategoriesList = categories
        };
        ViewBag.Action = "Edit";
        ViewBag.submitText = "Save";
        ViewBag.Thumbnail = _event.Thumbnail;
        ViewBag.RedirectCtrl = "Event";
        return View("~/Views/Event/Form.cshtml", editEvent);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(EventFormDto eventForm)
    {
        var categories = await _categoryService.GetAllCategories();
        eventForm.CategoriesList = categories;

        var userId = await _currentUserService.GetCurrentUserId();
        if (userId is null) return RedirectToAction("Login", "Account");

        var user = await _usersService.GetUserById(userId.ToString()!);
        eventForm.Owner = user!;

        ModelState.Remove("Owner");
        ModelState.Remove("CategoriesList");
        if (!ModelState.IsValid)
        {
            ViewBag.Action = "Edit";
            ViewBag.submitText = "Save";
            ViewBag.RedirectCtrl = "Event";
            return View("~/Views/Event/Form.cshtml", eventForm);
        };

        var _event = await _eventsService.GetEventInfoById(eventForm.Id!.Value);

        if (_event == null || _event.Owner.Id != userId.ToString()) return RedirectToAction("Index", "Home");

        if (!_event.IsOpen) return RedirectToAction("Index", "Home");

        var updatedEvent = await _eventsService.UpdateEvent(eventForm);

        if (updatedEvent == null)
        {
            TempData["Error"] = "An error occurred while updating the event";
            return View("~/Views/Event/Form.cshtml", eventForm);
        }

        string message = $"Dear {user!.Firstname} {user.Lastname},\n\n" +
                          $"Your event '{_event.Title}' has been successfully updated.\n\n" +
                          "Thank you for keeping your event details up-to-date!\n\n" +
                          "Best regards,\n" +
                          "The PuanConnect Team";

        await _emailService.SendCustomSubject(user.Email!, message, "Event Update Notification");


        return RedirectToAction("Index", "Event", new { id = eventForm.Id });
    }

    [Authorize]
    public async Task<ActionResult> Join([FromQuery(Name = "Id")] string eventId)
    {
        var userId = await _currentUserService.GetCurrentUserId();

        if (userId is null) return RedirectToAction("Login", "Account");

        var user = await _usersService.GetUserById(userId.ToString()!);

        var _event = await _eventsService.GetEventById(Guid.Parse(eventId));

        if (_event is null) return RedirectToAction("Index", "Home");

        // check event is close
        if (!_event.IsOpen)
        {
            TempData["Error"] = "The event is already closed";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        // check is close date is passed
        if (DateTime.Now.ToLocalTime() > _event.CloseDate.ToLocalTime())
        {
            TempData["Error"] = "The event is already closed, date";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        // check user is already joined
        var isAlreadyJoined = _event.Attendees.Any(a => a.User.Id == user!.Id && a.Status != AttendeeStatus.Left);

        if (isAlreadyJoined)
        {
            TempData["Error"] = "You are already joined this event";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        // check reputation is enough
        var userReputation = user!.ReputationPoint;

        if (userReputation < _event.MinReputation)
        {
            TempData["Error"] = "Your reputation is not enough to join this event";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        var result = await _eventsService.JoinEvent(_event, user!);

        if (result is null)
        {
            TempData["Error"] = "An error occurred while joining the event";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        string message = $"Dear {user.Firstname} {user.Lastname},\n\n" +
                          $"You have successfully joined the event '{_event.Title}'.\n\n" +
                          $"Event Details:\n" +
                          $"- Title: {_event.Title}\n" +
                          $"- Description: {_event.Description}\n" +
                          $"- Location: {_event.LocationName}\n" +
                          $"- Event Date: {_event.EventDate.ToString("F")}\n" +
                          $"- Close Date: {_event.CloseDate.ToString("F")}\n\n" +
                          $"Enjoy the event!\n\n" +
                          "Best regards,\n" +
                          "The PuanConnect Team";

        await _emailService.SendCustomSubject(user.Email!, message, "Event Join Confirmation");

        return RedirectToAction("Index", "Event", new { id = eventId });
    }

    [Authorize]
    public async Task<ActionResult> Leave([FromQuery(Name = "Id")] string eventId)
    {
        var userId = await _currentUserService.GetCurrentUserId();

        if (userId is null) return RedirectToAction("Login", "Account");
        var user = await _usersService.GetUserById(userId.ToString()!);

        var _event = await _eventsService.GetEventById(Guid.Parse(eventId));
        if (_event is null) return RedirectToAction("Index", "Home");

        // if event date is passed, you can't leave
        if (DateTime.Now.ToLocalTime() > _event.EventDate.ToLocalTime())
        {
            TempData["Error"] = "The event date is passed, you can't leave";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        // check user isn't joined and status is not left
        var isAlreadyJoined = _event.Attendees.Any(a => a.User.Id == user!.Id && a.Status != AttendeeStatus.Left);

        if (!isAlreadyJoined)
        {
            TempData["Error"] = "You are not joined this event";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        // owner can't leave
        if (_event.Owner.Id == user!.Id)
        {
            TempData["Error"] = "Owner can't leave the event";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        var result = await _eventsService.LeaveEvent(_event, user!);

        if (result is null)
        {
            TempData["Error"] = "An error occurred while leaving the event";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        string message = $"Dear {user.Firstname} {user.Lastname},\n\n" +
                         $"You have successfully left the event '{_event.Title}'.\n\n" +
                         "We hope to see you again in future events!\n\n" +
                         "Best regards,\n" +
                         "The PuanConnect Team";

        await _emailService.SendCustomSubject(user.Email!, message, "Event Leave Confirmation");

        return RedirectToAction("Index", "Event", new { id = eventId });
    }

    [Authorize]
    public async Task<ActionResult> Close([FromQuery(Name = "Id")] string eventId)
    {
        var userId = await _currentUserService.GetCurrentUserId();

        if (userId is null) return RedirectToAction("Login", "Account");

        var user = await _usersService.GetUserById(userId.ToString()!);

        var _event = await _eventsService.GetEventById(Guid.Parse(eventId));

        if (_event is null || _event.Owner.Id != user!.Id) return RedirectToAction("Index", "Home");

        var result = await _eventsService.CloseEvent(_event);

        if (result is null)
        {
            TempData["Error"] = "An error occurred while closing the event";
            return RedirectToAction("Index", "Event", new { id = _event.Id });
        }

        // Send mail message to the owner confirming event cancellation
        string ownerMessage = $"Dear {user.Firstname} {user.Lastname},\n\n" +
                              $"You have successfully closed the event '{_event.Title}'.\n\n" +
                              "Best regards,\n" +
                              "The PuanConnect Team";

        await _emailService.SendCustomSubject(user.Email!, ownerMessage, "Event Close Confirmation");

        return RedirectToAction("Index", "Home");
    }
}
