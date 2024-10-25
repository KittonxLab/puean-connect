using PuanConnect.Dtos.Event;
using PuanConnect.Dtos.User;
using PuanConnect.Models;

namespace PuanConnect.Interfaces;

public interface IUsersService
{
  Task<User?> GetUserByEmail(string email);
  Task<User?> GetUserById(string id);
  Task<User?> GetUserByPhoneNumber(string phoneNumber);
  Task<User?> GetUserByUsername(string username);
  void UpdateUser(string id, UpdateUserDto user);
  Task<User?> GetUserByEmailOrUsername(string emailOrUsername);
  Task<List<HistoryDto?>> GetHistory(string id);
}