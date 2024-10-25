using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuanConnect.Models;

[Table("EventAnswers")]
public class EventAnswer
{
    [Key]
    public Guid Id { get; set; }
    public required string Answer { get; set; }
    
    public required User User { get; set; }
    
    public required Guid QuestionId { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public EventAnswer()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}