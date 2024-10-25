using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using PuanConnect.Repositories;

namespace PuanConnect.Models;

[Table("Users")]
public class User : IdentityUser
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Firstname { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Lastname { get; set; }

    public string? Avatar { get; set; }

    public float ReputationPoint { get; set; }

    public string? Bio { get; set; }
    
    public ICollection<Event> HeldEvents { get; } = new List<Event>();
    
    public ICollection<Attendee> Attendances { get; } = new List<Attendee>();

    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User()
    {
        Avatar = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
        ReputationPoint = 100;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
