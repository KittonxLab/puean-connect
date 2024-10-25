using Microsoft.AspNetCore.Mvc;
using PuanConnect.Interfaces;

namespace PuanConnect.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : Controller
{
  private readonly ICategoryService _categoryService;
  private readonly IEventsService _eventsService;

  public CategoryController(ICategoryService categoryService, IEventsService eventsService)
  {
    _categoryService = categoryService;
    _eventsService = eventsService;
  }

  [HttpGet("all")]
  public async Task<ActionResult> GetAllCategories()
  {
    try
    {
      var result = await _categoryService.GetAllCategories();
      return Ok(result);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("event")]
  public async Task<ActionResult> GetEventsByCategory()
  {
    try
    {
      var result = await _eventsService.GetAllEvents();
      return Ok(result);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }
}
