using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PuanConnect.Database;
using PuanConnect.Dtos.Auth;
using PuanConnect.Interfaces;
using PuanConnect.Models;

namespace PuanConnect.Controllers;

public class AccountController : Controller
{
  private readonly UserManager<User> _userManager;
  private readonly SignInManager<User> _signInManager;
  private readonly AppDBContext _context;
  private readonly IUsersService _usersService;
  private readonly ICloudinaryService _cloudinaryService;
  private readonly IEmailService _emailService;


  public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AppDBContext context, IUsersService usersService, ICloudinaryService cloudinaryService, IEmailService emailService)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _context = context;
    _usersService = usersService;
    _cloudinaryService = cloudinaryService;
    _emailService = emailService;
  }

  [HttpGet]
  public IActionResult Login()
  {
    var response = new LoginDto();
    return View(response);
  }

  [HttpPost]
  public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
  {
    var badResult = View(loginDto);
    badResult.StatusCode = 400; // Bad request
    
    if (!ModelState.IsValid) return badResult;

    var user = await _usersService.GetUserByEmailOrUsername(loginDto.EmailOrUsername);

    if (user == null)
    {
      TempData["Error"] = "User not found";
      return badResult;
    }

    var result = await _signInManager.PasswordSignInAsync(user!, loginDto.Password, false, false);

    if (!result.Succeeded)
    {
      TempData["Error"] = "Invalid username or password";
      return badResult;
    }

    return Ok();
  }

  [HttpGet]
  public IActionResult Register()
  {
    var response = new RegisterDto();
    return View(response);
  }

  [HttpPost]
  public async Task<IActionResult> Register(RegisterDto registerDto)
  {
    if (!ModelState.IsValid) return View(registerDto);

    var emailExists = await _usersService.GetUserByEmail(registerDto.Email);

    if (emailExists != null)
    {
      TempData["Error"] = "Email already exists";
      return View(registerDto);
    }

    var usernameExists = await _usersService.GetUserByUsername(registerDto.UserName);

    if (usernameExists != null)
    {
      TempData["Error"] = "Username already exists";
      return View(registerDto);
    }

    var phoneNumberExists = await _usersService.GetUserByPhoneNumber(registerDto.PhoneNumber);

    if (phoneNumberExists != null)
    {
      TempData["Error"] = "Phone number already exists";
      return View(registerDto);
    }

    // change birthdate to UTC
    registerDto.BirthDate = registerDto.BirthDate.ToUniversalTime();

    var imageUrl = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";

    if (registerDto.Image != null)
    {
      imageUrl = await _cloudinaryService.UploadImageAsync(registerDto.Image!);
    }

    var user = new User
    {
      Email = registerDto.Email,
      UserName = registerDto.UserName,
      PhoneNumber = registerDto.PhoneNumber,
      BirthDate = registerDto.BirthDate,
      Firstname = registerDto.Firstname,
      Lastname = registerDto.Lastname,
      Bio = registerDto.Bio,
      Avatar = imageUrl
    };

    var result = await _userManager.CreateAsync(user, registerDto.Password);

    if (!result.Succeeded)
    {
      TempData["Error"] = "An error occurred";
      return View(registerDto);
    }

    await _signInManager.SignInAsync(user, false);

    string message = $"Dear {user.Firstname} {user.Lastname},\n\n" +
                     "Thank you for registering with our platform. Your account has been successfully created.\n\n" +
                     "You can now start exploring and participating in various events.\n\n" +
                     "Best regards,\n" +
                     "The PuanConnect Team";

    await _emailService.SendCustomSubject(user.Email, message, "Welcome to PuanConnect");

    return RedirectToAction("Index", "Home");
  }

  public async Task<IActionResult> Logout()
  {
    await _signInManager.SignOutAsync();
    return RedirectToAction("Index", "Home");
  }
}