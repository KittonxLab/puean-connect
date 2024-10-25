using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PuanConnect.Dtos.User;
using PuanConnect.Interfaces;
using PuanConnect.Models;

public class UserController : Controller
{
    private readonly IUsersService _usersService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IEmailService _emailService;

    public UserController(IUsersService usersService, IMapper mapper, ICurrentUserService currentUserService, UserManager<User> userManager, ICloudinaryService cloudinaryService, IEmailService emailService)
    {
        _userManager = userManager;
        _usersService = usersService;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
        _emailService = emailService;
    }

    [Authorize]
    public async Task<ActionResult> Profile()
    {
        var userId = await _currentUserService.GetCurrentUserId();

        if (userId is null) return RedirectToAction("Login", "Account");

        var user = await _usersService.GetUserById(userId.ToString()!);
        var userProfile = _mapper.Map<User, UserProfileDto>(user!);
        ViewBag.isOwner = true;

        return View(userProfile);
    }
    
    public async Task<ActionResult> People([FromQuery(Name = "id")] string id)
    {
        if (id is null) return RedirectToAction("Index", "Home");

        var user = await _usersService.GetUserById(id!);
        var userProfile = _mapper.Map<User, UserProfileDto>(user!);
        ViewBag.isOwner = false;

        return View("~/Views/User/Profile.cshtml", userProfile);
    }

    [Authorize]
    [HttpGet("user/profile/edit")]
    public async Task<ActionResult> Edit()
    {
        var userId = await _currentUserService.GetCurrentUserId();

        if (userId is null) return RedirectToAction("Login", "Account");

        var user = await _usersService.GetUserById(userId.ToString()!);
        var userEditor = _mapper.Map<User, UpdateUserDto>(user!);

        return View(userEditor);
    }

    [Authorize]
    [HttpPost("user/profile/edit")]
    public async Task<ActionResult> Edit(UpdateUserDto userEditor)
    {
        if (!ModelState.IsValid) return View(userEditor);

        var userId = await _currentUserService.GetCurrentUserId();
        if (userId is null) return RedirectToAction("Login", "Account");

        var user = await _usersService.GetUserById(userId.ToString()!);
        if (user is null) return RedirectToAction("Login", "Account");

        var emailExists = await _usersService.GetUserByEmail(userEditor.Email!);

        if (emailExists != null && emailExists.Id != user.Id)
        {
            TempData["Error"] = "Email already exists";
            return View(userEditor);
        }

        var usernameExists = await _usersService.GetUserByUsername(userEditor.Username!);

        if (usernameExists != null && usernameExists.Id != user.Id)
        {
            TempData["Error"] = "Username already exists";
            return View(userEditor);
        }

        var phoneNumberExists = await _usersService.GetUserByPhoneNumber(userEditor.PhoneNumber!);

        if (phoneNumberExists != null && phoneNumberExists.Id != user.Id)
        {
            TempData["Error"] = "Phone number already exists";
            return View(userEditor);
        }

        // birthdate to utc
        userEditor.BirthDate = userEditor.BirthDate?.ToUniversalTime();

        if (userEditor.Image != null)
        {
            // delete old image
            if (user.Avatar != null)
            {
                await _cloudinaryService.DeleteImageAsync(user.Avatar);
            }

            var avatar = await _cloudinaryService.UploadImageAsync(userEditor.Image);
            userEditor.Avatar = avatar;
        }

        userEditor.Avatar = userEditor.Avatar ?? user.Avatar;

        // if password is not null, update password 
        if (!string.IsNullOrEmpty(userEditor.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, userEditor.Password);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Password reset failed";
                return View(userEditor);
            }
        }

        await _userManager.UpdateAsync(_mapper.Map(userEditor, user, opt => opt.AfterMap((src, dest) =>
        {
            dest.UpdatedAt = DateTime.UtcNow;
        }))!);

        string message = $"Dear {user.Firstname} {user.Lastname},\n\n" +
                          "Your profile has been successfully updated.\n\n" +
                          "Thank you for keeping your profile up-to-date!\n\n" +
                          "Best regards,\n" +
                          "The PuanConnect Team";

        await _emailService.SendCustomSubject(user.Email!, message, "Profile Update Notification");


        return RedirectToAction("Profile", "User");
    }
}