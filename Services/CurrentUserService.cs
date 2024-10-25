using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PuanConnect.Interfaces;
using PuanConnect.Models;

namespace PuanConnect.Services;

public class CurrentUserService : ICurrentUserService
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly UserManager<User> _userManager;

  public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
  {
    _httpContextAccessor = httpContextAccessor;
    _userManager = userManager;
  }

  public async Task<Guid?> GetCurrentUserId()
  {
    var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User!);
    
    return currentUser?.Id == null ? null : Guid.Parse(currentUser.Id);
  }
}