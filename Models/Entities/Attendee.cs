using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuanConnect.Models;

[Table("Attendees")]
public class Attendee
{
    [Key]
    public Guid Id { get; set; }
    public required Guid EventId { get; set; }

    public required Event Event { get; set; }

    public required User User { get; set; }

    public required bool IsApproved { get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Attendee()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Status = AttendeeStatus.Pending;
    }
}