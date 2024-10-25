using Microsoft.EntityFrameworkCore;
using PuanConnect.Database;
using PuanConnect.Dtos.Event;
using PuanConnect.Dtos.User;
using PuanConnect.Interfaces;
using PuanConnect.Models;

namespace PuanConnect.Repositories;

public class UsersRepository : IUsersRepository
{
  private readonly AppDBContext _dbContext;

  public UsersRepository(AppDBContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<User> CreateUser(User user)
  {
    await _dbContext.Users.AddAsync(user);
    await _dbContext.SaveChangesAsync();
    return user;
  }

  public async Task<User?> GetUserById(string id)
  {
    return await _GetUserQuery().FirstOrDefaultAsync(u => u.Id == id);
  }

  public async Task<User?> GetUserByUsername(string username)
  {
    return await _GetUserQuery().FirstOrDefaultAsync(u => u.UserName == username);
  }

  public async Task<User?> GetUserByEmail(string email)
  {
    return await _GetUserQuery().FirstOrDefaultAsync(u => u.Email == email);
  }

  public async Task<User?> GetUserByPhoneNumber(string phoneNumber)
  {
    return await _GetUserQuery().FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
  }

  public async Task<List<User>> GetUsers()
  {
    return await _dbContext.Users.ToListAsync();
  }

  public async void UpdateUser(string id, UpdateUserDto updateUserDto)
  {
    var user = GetUserById(id).Result;

    if (user == null)
    {
      throw new Exception("User not found");
    }

    var dtoProperties = updateUserDto.GetType().GetProperties().Where(prop => prop.GetValue(updateUserDto) != null);

    foreach (var prop in dtoProperties)
    {
      var userProp = user.GetType().GetProperty(prop.Name);
      if (userProp != null && userProp.CanWrite)
      {
        userProp.SetValue(user, prop.GetValue(updateUserDto));
      }
    }

    await _dbContext.SaveChangesAsync();
  }


  public async Task<User?> GetUserByEmailOrUsername(string emailOrUsername)
  {
    return await _dbContext.Users.FirstOrDefaultAsync(
      u => u.Email == emailOrUsername || u.UserName == emailOrUsername
    );
  }

  private IQueryable<User> _GetUserQuery()
  {
    var query = _dbContext.Users
                                .Include(u => u.HeldEvents)
                                .ThenInclude(e => e.Category)
                                .Include(u => u.Attendances)
                                  .ThenInclude(e => e.Event);
    return query;
  }

  public async Task<User?> GetUserHistory(string id)
  {
    var history = await _GetUserQuery().FirstOrDefaultAsync(u => u.Id == id);

    return history;
  }

  public async Task<List<HistoryDto>> GetHeldEvent(string id, DateTime currentTimeUtc)
  {
    var user = await GetUserHistory(id);
    var heldEvents = user!.HeldEvents
    .Where(heldEvent => heldEvent.EventDate <= currentTimeUtc)
    .Select(heldEvent => new HistoryDto
    {
      Id = heldEvent.Id,
      Title = heldEvent.Title,
      CloseDate = heldEvent.CloseDate,
      EventDate = heldEvent.EventDate,
      IsOpen = heldEvent.IsOpen,
      Status = "Author",
      IsOwner = true
    })
    .ToList();

    return heldEvents;
  }

  public async Task<IEnumerable<HistoryDto>> GetAttendances(string id, DateTime currentTimeUtc)
  {
    var user = await GetUserHistory(id);

    var filteredAttendances = user!.Attendances
        .Where(a => !user.HeldEvents.Any(h => h.Id == a.Event.Id))
        .Where(a => a.Event.EventDate <= currentTimeUtc)
        .Select(a => new HistoryDto
        {
          Id = a.Event.Id,
          Title = a.Event.Title,
          CloseDate = a.Event.CloseDate,
          EventDate = a.Event.EventDate,
          IsOpen = a.Event.IsOpen,
          Status = a.Status,
          IsOwner = false
        });

    return filteredAttendances;
  }
  
  public async Task<int> GainUsersRPWorker() {
    await _dbContext.Users.Where(
      user => user.ReputationPoint < 100
    )
    .ExecuteUpdateAsync(
      s => s.SetProperty(
          user => user.ReputationPoint, 
          user => Math.Min(100, user.ReputationPoint + 15)
      )
    );
    return 0;
  }
}