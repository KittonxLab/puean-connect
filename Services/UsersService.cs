using AutoMapper;
using PuanConnect.Dtos.Attendee;
using PuanConnect.Dtos.Event;
using PuanConnect.Dtos.User;
using PuanConnect.Interfaces;
using PuanConnect.Models;

namespace PuanConnect.Services;

public class UsersService : IUsersService
{
  private readonly IUsersRepository _usersRepository;
  private readonly IMapper _mapper;

  public UsersService(IUsersRepository usersRepository, IMapper mapper)
  {
    _usersRepository = usersRepository;
    _mapper = mapper;
  }

  public async Task<User?> GetUserByEmail(string email)
  {
    return await _usersRepository.GetUserByEmail(email);
  }

  public async Task<User?> GetUserById(string id)
  {
    return await _usersRepository.GetUserById(id);
  }

  public async Task<User?> GetUserByPhoneNumber(string phoneNumber)
  {
    return await _usersRepository.GetUserByPhoneNumber(phoneNumber);
  }

  public async Task<User?> GetUserByUsername(string username)
  {
    return await _usersRepository.GetUserByUsername(username);
  }

  public void UpdateUser(string id, UpdateUserDto user)
  {
    // hash password
    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
    user.Password = hashedPassword;

    // change birthdate to UTC
    user.BirthDate = user.BirthDate?.ToUniversalTime();

    _usersRepository.UpdateUser(id, user);
  }

  public async Task<User?> GetUserByEmailOrUsername(string emailOrUsername)
  {
    return await _usersRepository.GetUserByEmailOrUsername(emailOrUsername);
  }

  public async Task<List<HistoryDto?>> GetHistory(string id)
  {
    var currentTimeUtc = DateTime.UtcNow;

    var historyDtos = new List<HistoryDto>();

    historyDtos = await _usersRepository.GetHeldEvent(id, currentTimeUtc);

    var filteredAttendances = await _usersRepository.GetAttendances(id, currentTimeUtc);

    historyDtos.AddRange(filteredAttendances);

    if (historyDtos == null)
    {
      throw new Exception("No events or attendances found(null) for the user.");
    }
    historyDtos = historyDtos.OrderByDescending(h => h.EventDate).ToList();

    return historyDtos!;
  }
}