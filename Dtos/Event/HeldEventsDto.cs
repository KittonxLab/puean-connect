using PuanConnect.Dtos.Category;

namespace PuanConnect.Dtos.Event;

public class HeldEventsDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }

    public required DateTime EventDate { get; set; }

    public required DateTime CloseDate { get; set; }

    public required int MaxParticipants { get; set; }

    public required int CurrentParticipants { get; set; }

    public required int ApprovedParticipants { get; set; }

    public required CategoryDto Category { get; set; }
}