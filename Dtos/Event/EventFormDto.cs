using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PuanConnect.Dtos.Category;

namespace PuanConnect.Dtos.Event;

public class EventFormDto
{
    public Guid? Id { get; set; }
    
    [Display(Name = "Title")]
    [Required(ErrorMessage = "we required event title")]
    [StringLength(50, MinimumLength = 1)]
    public string Title { get; set; }

    [Display(Name = "About")]
    public string? Description { get; set; }

    [Display(Name = "Location info")]
    public string? LocationName { get; set; }

    public float? LocationLat { get; set; }

    public float? LocationLng { get; set; }

    [Display(Name = "Upload Image")]
    public IFormFile? Thumbnail { get; set; }

    [Display(Name = "Event Date")]
    [Required(ErrorMessage = "we required event date")]
    public DateTime EventDate { get; set; }

    [Display(Name = "Close Date")]
    [Required(ErrorMessage = "we required closing date")]
    public DateTime CloseDate { get; set; }

    [Display(Name = "Tags")]
    public string? Tags { get; set; }

    [Display(Name = "RP")]
    [Required(ErrorMessage = "we required minimum RP point")]
    public float MinReputation { get; set; }

    [Display(Name = "Seat")]
    [Required(ErrorMessage = "we required seat")]
    [Range(1, 100)]
    public int MaxParticipants { get; set; }

    [Display(Name = "Category")]
    [Required(ErrorMessage = "we required category")]
    public string CategoryId { get; set; }

    public required List<CategoryDto> CategoriesList { get; set; }

    public required PuanConnect.Models.User Owner { get; set; }
}