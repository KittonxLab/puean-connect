namespace PuanConnect.Dtos.Attendee;

public class EventAttendantDto
{
  public Guid Id { get; set; }
  public required AttendeeDto User { get; set; }

  public required bool isApproved { get; set; }

  public required string Status { get; set; }

  public required DateTime UpdatedAt { get; set; }

}