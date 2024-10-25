using PuanConnect.Models;
using PuanConnect.Dtos.User;
using PuanConnect.Dtos.Event;

namespace PuanConnect.Interfaces;

public interface IUsersRepository
{
  Task<User> CreateUser(User user);
  Task<User?> GetUserById(string id);
  Task<User?> GetUserByUsername(string username);
  Task<User?> GetUserByEmail(string email);
  Task<User?> GetUserByPhoneNumber(string phoneNumber);
  Task<List<User>> GetUsers();
  void UpdateUser(string id, UpdateUserDto user);
  Task<User?> GetUserByEmailOrUsername(string emailOrUsername);
  Task<User?> GetUserHistory(string id);
  Task<List<HistoryDto>> GetHeldEvent(string id, DateTime currentTimeUtc);
  Task<IEnumerable<HistoryDto>> GetAttendances(string id, DateTime currentTimeUtc);
  
  Task<int> GainUsersRPWorker();
}