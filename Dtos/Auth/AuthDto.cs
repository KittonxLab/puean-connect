using PuanConnect.Dtos.User;

namespace PuanConnect.Dtos.Auth;

public class AuthDto
{
  public required CredentialsDto Credentials { get; set; }

  public required UserProfileDto Profile { get; set; }
}
