using PuanConnect.Dtos.Event;
namespace PuanConnect.Dtos.Attendee;

public class AttendancesDto
{
    public required Guid Id { get; set; }
    
    public required Guid EventId { get; set; }
    
    public required AttendEventsDto Event { get; set; }
    
    public required bool IsApproved { get; set; }
    
    public required string Status { get; set; }
    
    public required DateTime UpdatedAt { get; set; }
}