using Microsoft.AspNetCore.Mvc;
using PuanConnect.Interfaces;
using PuanConnect.Dtos.Event;
using PuanConnect.Dtos.Attendee;
using PuanConnect.Models;

namespace PuanConnect.Controllers;

public class GroupsController : Controller
{
  private readonly ILogger<GroupsController> _logger;
  private readonly IEventsService _eventsService;
  private readonly ICurrentUserService _currentUserService;
  private readonly IUsersService _usersService;

  public GroupsController(ILogger<GroupsController> logger, IEventsService eventsService, ICurrentUserService currentUserService, IUsersService usersService)
  {
    _logger = logger;
    _eventsService = eventsService;
    _currentUserService = currentUserService;
    _usersService = usersService;
  }

  public class GroupData
  {
    public GroupsDto groupsDto;
    public string status;
  }

  public async Task<IActionResult> Index()
  {
    // try
    // {
    var userId = await _currentUserService.GetCurrentUserId();
    if (userId == null) return RedirectToAction("Login", "Account");

    List<GroupsDto> events = _eventsService.GetGroups(userId.ToString()!).Result;

    List<GroupData> groupDatas = new List<GroupData>();

    foreach (GroupsDto e in events)
    {
      string status = "";

      foreach (EventAttendantDto attendee in e.Attendees)
      {
        if (attendee.User.Id == userId.ToString())
        {
          status = attendee.Status;
          break;
        }
      }

      groupDatas.Add(new GroupData()
      {
        groupsDto = e,
        status = status
      });
    }

    return View(groupDatas);
    // }
    // catch (Exception ex)
    // {
    //   Console.Write("Internal server error: ", ex);
    //   Console.WriteLine("laskdfjlsdlkfjsjlksdfkljjlkdsfdkjll");
    //   return RedirectToAction("Index", "Home");
    // }
  }
}
