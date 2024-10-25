using System.ComponentModel.DataAnnotations;

namespace PuanConnect.Dtos.Auth;

public class RegisterDto
{
  [Required(ErrorMessage = "Username is required")]
  [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters long")]
  public string UserName { get; set; }

  [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
  [Required(ErrorMessage = "Password is required")]
  public string Password { get; set; }

  [Required(ErrorMessage = "Email is required")]
  [EmailAddress(ErrorMessage = "Invalid email address")]
  public string Email { get; set; }

  [Required(ErrorMessage = "Firstname is required")]
  [StringLength(50, MinimumLength = 1, ErrorMessage = "Firstname must be between 1 and 50 characters long")]
  public string Firstname { get; set; }

  [Required(ErrorMessage = "Lastname is required")]
  [StringLength(50, MinimumLength = 1, ErrorMessage = "Lastname must be between 1 and 50 characters long")]
  public string Lastname { get; set; }

  [Required(ErrorMessage = "Phone number is required")]
  [Phone(ErrorMessage = "Invalid phone number")]
  [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 characters long")]
  public string PhoneNumber { get; set; }
  public string? Bio { get; set; }

  [Required(ErrorMessage = "Birth date is required")]
  [DataType(DataType.Date)]
  public DateTime BirthDate { get; set; }

  public IFormFile? Image { get; set; }
  
}