using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PuanConnect.Dtos.Event;
using PuanConnect.Interfaces;
using PuanConnect.Models;

namespace PuanConnect.Controllers;

public class HistoryController : Controller
{
  private readonly ILogger<HistoryController> _logger;
  private readonly IUsersService _usersService;
  private readonly ICurrentUserService _currentUserService;

  public HistoryController(ILogger<HistoryController> logger, IUsersService usersService, ICurrentUserService currentUserService)
  {
    _logger = logger;
    _usersService = usersService;
    _currentUserService = currentUserService;
  }

  public async Task<IActionResult> Index()
  {
    try
    {
      var userId = await _currentUserService.GetCurrentUserId();
      if (userId is null) return RedirectToAction("Login", "Account");

      var history = _usersService.GetHistory(userId.ToString()).Result;

      return View(history);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An error occurred while retrieving user history.");

      return RedirectToAction("Index", "Home");
    }
  }
}
