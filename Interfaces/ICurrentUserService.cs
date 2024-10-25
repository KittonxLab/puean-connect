using PuanConnect.Dtos.User;

namespace PuanConnect.Interfaces;

public interface ICurrentUserService
{
  Task<Guid?> GetCurrentUserId();
}