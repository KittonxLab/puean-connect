using Microsoft.AspNetCore.Mvc;

namespace PuanConnect.Controllers;
using Microsoft.AspNetCore.Mvc;
using PuanConnect.Dtos.Event;
using PuanConnect.Interfaces;
using PuanConnect.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;


public class ApprovalController : Controller
{
  private readonly ILogger<ApprovalController> _logger;
  private readonly IEventsService _eventsService;
  private readonly ICurrentUserService _currentUserService;
  private readonly IUsersService _usersService;
  private readonly IEmailService _emailService;

  public ApprovalController(ILogger<ApprovalController> logger, IEventsService eventsService, ICurrentUserService currentUserService, IUsersService usersService, IEmailService emailService)
  {
    _currentUserService = currentUserService;
    _logger = logger;
    _usersService = usersService;
    _eventsService = eventsService;
    _emailService = emailService;
  }

  [Authorize]
  public async Task<ActionResult> Index([FromQuery(Name = "id")] string id, [FromQuery(Name = "method")] string method)
  {
    try
    {
      var eventId = Guid.Parse(id);
      var userId = await _currentUserService.GetCurrentUserId();

      if (userId is null)
      {
        return RedirectToAction("Index", "Home");
      }

      ViewBag.sortMethod = method ?? "all";

      EventDto _event = _eventsService.GetEventInfoById(eventId).Result;

      ViewBag.userId = userId.ToString();

      if (_event!.Owner.Id == userId!.ToString())
      {
        return View(_event);
      }

      return RedirectToAction("Index", "Home");

    }
    catch
    {
      return RedirectToAction("Index", "Home");
    }
  }

  [Authorize]
  public async Task<ActionResult> Approve([FromQuery(Name = "EventId")] string eventId, [FromQuery(Name = "UserId")] string userToApproveId)
  {
    var userId = await _currentUserService.GetCurrentUserId();

    if (userId is null) return RedirectToAction("Login", "Account");

    var user = await _usersService.GetUserById(userId.ToString()!);

    var _event = await _eventsService.GetEventById(Guid.Parse(eventId));

    if (_event!.ApprovedParticipants + 1 > _event.MaxParticipants)
    {

      TempData["Error"] = "An error occurred cannot approve exceed max participants";
      return RedirectToAction("Index", "Approval", new { id = _event.Id });
    }

    if (_event is null || _event.Owner.Id != user!.Id) return RedirectToAction("Index", "Home");

    var userToApprove = await _usersService.GetUserById(userToApproveId);

    if (userToApprove is null) return RedirectToAction("Index", "Home");

    var attendee = await _eventsService.GetAttendeeByEventIdAndUserId(_event.Id, userToApprove.Id);

    if (attendee is null || attendee.IsApproved) return RedirectToAction("Index", "Approval", new { id = _event.Id }); 

    var result = await _eventsService.ApproveAttendee(_event, userToApprove);

    if (result is null)
    {
      Console.WriteLine("Error approving user");
      TempData["Error"] = "An error occurred while closing the event";
      return RedirectToAction("Index", "Approval", new { id = _event.Id });
    }

    string message = $"Dear {userToApprove.Firstname} {userToApprove.Lastname},\n\n" +
                     $"You have been approved by the owner of the event '{_event.Title}'.\n\n" +
                     "See you at the event!\n" +
                     "Best regards,\n" +
                     "The PuanConnect Team";

    await _emailService.SendCustomSubject(userToApprove.Email!, message, "Event Approve Notification");

    return RedirectToAction("Index", "Approval", new { id = eventId });
  }

  [Authorize]
  public async Task<ActionResult> DisApprove([FromQuery(Name = "EventId")] string eventId, [FromQuery(Name = "UserId")] string userToApproveId)
  {
    var userId = await _currentUserService.GetCurrentUserId();

    if (userId is null) return RedirectToAction("Login", "Account");

    var user = await _usersService.GetUserById(userId.ToString()!);

    var _event = await _eventsService.GetEventById(Guid.Parse(eventId));

    if (_event is null || _event.Owner.Id != user!.Id) return RedirectToAction("Index", "Home");

    var userToApprove = await _usersService.GetUserById(userToApproveId);

    if (userToApprove is null) return RedirectToAction("Index", "Home");

    var attendee = await _eventsService.GetAttendeeByEventIdAndUserId(_event.Id, userToApprove.Id);

    if (attendee is null || !attendee.IsApproved) return RedirectToAction("Index", "Approval", new { id = _event.Id }); 

    var result = await _eventsService.DisapproveAttendee(_event, userToApprove, user);

    if (result is null)
    {
      Console.WriteLine("Error approving user");
      TempData["Error"] = "An error occurred while closing the event";
      return RedirectToAction("Index", "Approval", new { id = _event.Id });
    }

    string message = $"Dear {userToApprove.Firstname} {userToApprove.Lastname},\n\n" +
                     $"You have been disapproved by the owner of the event '{_event.Title}'.\n\n" +
                     "We are sorry for the inconvenience. Stay for a little we may approve you again later!\n" +
                     "Best regards,\n" +
                     "The PuanConnect Team";

    await _emailService.SendCustomSubject(userToApprove.Email!, message, "Event Cancel Notification");

    return RedirectToAction("Index", "Approval", new { id = eventId });
  }
}
