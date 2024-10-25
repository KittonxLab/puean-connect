using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.SignalR;

namespace PuanConnect.Models;

[Table("EventQuestions")]
public class EventQuestion 
{
    [Key]
    public Guid Id { get; set; }
    public required string Question { get; set; }
    
    public required Guid EventId { get; set; }
    
    public ICollection<EventAnswer> AnswersList { get; } = new List<EventAnswer>();
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public EventQuestion()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}