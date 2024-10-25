using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PuanConnect.Interfaces;
using PuanConnect.Dtos.Event;
using PuanConnect.Models;
using PuanConnect.Dtos.User;
using Microsoft.AspNetCore.Authorization;

namespace PuanConnect.Controllers;
public class YourEventsController : Controller
{
  private readonly ILogger<YourEventsController> _logger;
  private readonly ICurrentUserService _currentUserService;
  private readonly IEventsService _eventsService;
  private readonly IUsersService _usersService;
  public YourEventsController(ILogger<YourEventsController> logger, IEventsService eventsService, ICurrentUserService currentUserService, IUsersService usersService)
  {
    _currentUserService = currentUserService;
    _logger = logger;
    _eventsService = eventsService;
    _usersService = usersService;
  }

  public async Task<ActionResult> Index()
  {
    var userId = await _currentUserService.GetCurrentUserId();

    if (userId == null) return RedirectToAction("Login", "Account");

    List<YourEventsDto> events = _eventsService.GetYourEvents(userId.ToString()!).Result;

    return View(events);
  }

  [Authorize]
  public async Task<ActionResult> Close([FromQuery(Name = "Id")] string eventId)
  {
    var userId = await _currentUserService.GetCurrentUserId();

    if (userId is null) return RedirectToAction("Login", "Account");

    var user = await _usersService.GetUserById(userId.ToString()!);

    var _event = await _eventsService.GetEventById(Guid.Parse(eventId));

    if (_event is null || _event.Owner.Id != user!.Id) return RedirectToAction("Index", "Home");

    var result = await _eventsService.CloseEvent(_event!);

    if (result is null)
    {
      TempData["Error"] = "An error occurred while closing the event";
      return RedirectToAction("Index", "Event", new { id = _event!.Id });
    }

    return RedirectToAction("Index");
  }
}
