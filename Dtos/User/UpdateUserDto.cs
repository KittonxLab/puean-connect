using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PuanConnect.Dtos.User;

public class UpdateUserDto
{
  
  [Display(Name="Username")]
  [StringLength(50, MinimumLength = 4)]
  public string? Username { get; set; }

  [Display(Name="Password")]
  [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
  public string? Password { get; set; }

  [Display(Name="Firstname")]
  [StringLength(50, MinimumLength = 1)]
  public string? Firstname { get; set; }
  
  [Display(Name="Lastname")]
  [StringLength(50, MinimumLength = 1)]
  public string? Lastname { get; set; }
  
  [Display(Name="Email")]
  [EmailAddress]
  public string? Email { get; set; }
  
  [Phone]
  [Display(Name="Phone number")]
  [StringLength(10, MinimumLength = 10)]
  public string? PhoneNumber { get; set; }

  [Display(Name="Bio")]
  public string? Bio { get; set; }

  public string? Avatar { get; set; }
  
  public IFormFile? Image { get; set; }

  [DataType(DataType.Date)]
  [Display(Name="Date of birth")]
  [Required(ErrorMessage = "Date of birth is required")]
  public DateTime? BirthDate { get; set; }
}