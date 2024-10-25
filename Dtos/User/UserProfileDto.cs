using PuanConnect.Dtos.Event;
using PuanConnect.Dtos.Attendee;

namespace PuanConnect.Dtos.User;

public class UserProfileDto
{
  public required string Id { get; set; }

  public required string UserName { get; set; }

  public required string Email { get; set; }

  public required string Firstname { get; set; }

  public required string Lastname { get; set; }

  public required string PhoneNumber { get; set; }

  public string? Bio { get; set; }

  public string? Avatar { get; set; }

  public required DateTime BirthDate { get; set; }

  public required DateTime CreatedAt { get; set; }

  public required float ReputationPoint { get; set; }

  public required List<HeldEventsDto> HeldEvents { get; set; }
  
  public required List<AttendancesDto> Attendances { get; set; }
}