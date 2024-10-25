using Microsoft.AspNetCore.Mvc;

namespace PuanConnect.Controllers;

public class LandingController : Controller
{
    private readonly ILogger<LandingController> _logger;

    public LandingController(ILogger<LandingController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}