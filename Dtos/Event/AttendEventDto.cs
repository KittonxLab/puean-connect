using PuanConnect.Dtos.Category;

namespace PuanConnect.Dtos.Event;

public class AttendEventsDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required DateTime EventDate { get; set; }
    public required DateTime CloseDate { get; set; }
    public bool IsOpen { get; set; }
    public required CategoryDto Category { get; set; }
}