using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuanConnect.Models;

[Table("Events")]
public class Event
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public required string Title { get; set; }

    public string? Description { get; set; }

    public string? LocationName { get; set; }

    public float? LocationLat { get; set; }

    public float? LocationLng { get; set; }

    public string? Thumbnail { get; set; }

    [Required]
    public required DateTime EventDate { get; set; }

    [Required]
    public required DateTime CloseDate { get; set; }

    public bool IsOpen { get; set; }

    public string[]? Tags { get; set; }

    [Required]
    public required float MinReputation { get; set; }

    [Required]
    public required int MaxParticipants { get; set; }

    [Required]
    public required int CurrentParticipants { get; set; }

    [Required]
    public required int ApprovedParticipants { get; set; }

    [Required]
    public required User Owner { get; set; }

    [Required]
    public required Guid CategoryId { get; set; }

    [Required]
    public Category Category { get; set; }

    public ICollection<Attendee> Attendees { get; } = new List<Attendee>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Event()
    {
        IsOpen = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Thumbnail = "https://cdn.discordapp.com/attachments/1201438198425980998/1213872327872348181/Puan-connect-bg.png?ex=65f70db3&is=65e498b3&hm=4f2f8d9c32553e83d009244e2ef71066d81d57924759be06c5539e56a6c8f311&";
    }
}