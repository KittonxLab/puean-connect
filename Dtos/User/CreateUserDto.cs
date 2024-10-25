using System.ComponentModel.DataAnnotations;

namespace PuanConnect.Dtos.User;

public class CreateUserDto
{
  [Required]
  [StringLength(50, MinimumLength = 4)]
  public required string Username { get; set; }

  [Required]
  public required string Password { get; set; }

  [Required]
  [EmailAddress]
  public required string Email { get; set; }

  [Required]
  [StringLength(50, MinimumLength = 1)]
  public required string Firstname { get; set; }

  [Required]
  [StringLength(50, MinimumLength = 1)]
  public required string Lastname { get; set; }

  [Required]
  [Phone]
  [StringLength(10, MinimumLength = 10)]
  public required string PhoneNumber { get; set; }

  public string? Bio { get; set; }

  [Url]
  public string? Avatar { get; set; }

  [Required]
  [DataType(DataType.Date)]
  public required DateTime BirthDate { get; set; }
}
