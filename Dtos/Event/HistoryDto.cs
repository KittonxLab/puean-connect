using PuanConnect.Dtos.Attendee;
using PuanConnect.Dtos.Category;

namespace PuanConnect.Dtos.Event;

public class HistoryDto
{

     public required Guid Id { get; set; }
     public required string Title { get; set; }
     public required DateTime CloseDate { get; set; }

     public required DateTime EventDate { get; set; }
     public bool IsOpen { get; set; }
     public required string Status { get; set; }
     public required bool IsOwner {get; set;}

}