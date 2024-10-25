using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PuanConnect.Interfaces;
using PuanConnect.Models;

namespace PuanConnect.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly IEventsService _eventsService;

  public HomeController(ILogger<HomeController> logger, IEventsService eventsService)
  {
    _logger = logger;
    _eventsService = eventsService;
  }

  public IActionResult Index()
  {
    List<Dtos.Event.EventDto> events = _eventsService.GetAllEvents().Result;
    // sort ascending (close to now first)
    events.Sort((x, y) => DateTime.Compare(x.EventDate, y.EventDate));

    return View(events);
  }

  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
