using System.ComponentModel.DataAnnotations;
using PuanConnect.Dtos.Attendee;
using PuanConnect.Dtos.Category;
using PuanConnect.Models;

namespace PuanConnect.Dtos.Event;

public class EventDto
{
  [Required]
  public required Guid Id { get; set; }

  [Required]
  [StringLength(50, MinimumLength = 1)]
  public required string Title { get; set; }

  public string? Description { get; set; }

  public string? LocationName { get; set; }

  public float? LocationLat { get; set; }

  public float? LocationLng { get; set; }

  public string? Thumbnail { get; set; }

  [Required]
  [DataType(DataType.Date)]
  public required DateTime EventDate { get; set; }

  [Required]
  [DataType(DataType.Date)]
  public required DateTime CloseDate { get; set; }

  [Required]
  public bool IsOpen { get; set; }

  public string[]? Tags { get; set; }

  [Required]
  public required float MinReputation { get; set; }

  [Required]
  public required int MaxParticipants { get; set; }

  [Required]
  public required int CurrentParticipants { get; set; }

  [Required]
  public required int ApprovedParticipants { get; set; }

  [Required]
  public required AttendeeDto Owner { get; set; }

  [Required]
  public required CategoryDto Category { get; set; }

  [Required]
  public required List<EventAttendantDto> Attendees { get; set; }
}