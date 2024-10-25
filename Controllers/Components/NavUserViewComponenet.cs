using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PuanConnect.Interfaces;
using PuanConnect.Models;
using PuanConnect.Dtos.User;

namespace PuanConnect.Controllers;

public class NavUserViewComponent : ViewComponent
{
    private readonly IUsersService _usersService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    
    public NavUserViewComponent(IUsersService userService, ICurrentUserService currentUserService, IMapper mapper) {
        _usersService = userService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    public async Task<IViewComponentResult> InvokeAsync() {
        var userId = await _currentUserService.GetCurrentUserId();
        ViewBag.isAuthen = false;
        
        if(userId is not null) {
            ViewBag.isAuthen = true;
        }
        
        var user = await _usersService.GetUserById(userId.ToString());
        var userProfile = _mapper.Map<User, UserProfileDto>(user!);
        return View(userProfile!);
    }
}
