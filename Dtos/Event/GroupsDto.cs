
using System.Security.Cryptography.X509Certificates;
using PuanConnect.Dtos.Attendee;

namespace PuanConnect.Dtos.Event;

public class GroupsDto
{
  public required Guid Id { get; set; }

  public required string Title { get; set; }

  public required DateTime EventDate { get; set; }

  public required DateTime CloseDate { get; set; }

  public bool IsOpen { get; set; }

  public required List<EventAttendantDto> Attendees { get; set; }
}