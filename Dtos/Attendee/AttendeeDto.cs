namespace PuanConnect.Dtos.Attendee;

public class AttendeeDto
{
  public required string Id { get; set; }
  public required string UserName { get; set; }
  public string? Avatar { get; set; }
  public required float ReputationPoint { get; set; }

}