using System.ComponentModel.DataAnnotations;

namespace PuanConnect.Dtos.Auth;

public class LoginDto
{
  [Required(ErrorMessage = "Email or username is required")]
  public string EmailOrUsername { get; set; }

  [Required(ErrorMessage = "Password is required")]
  [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
  [DataType(DataType.Password)]
  public string Password { get; set; }
}
