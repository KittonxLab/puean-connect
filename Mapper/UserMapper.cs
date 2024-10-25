using AutoMapper;
using PuanConnect.Dtos.Auth;
using PuanConnect.Dtos.Event;
using PuanConnect.Dtos.User;
using PuanConnect.Models;

namespace PuanConnect.Mapper;

public class UserMapper : Profile
{
  public UserMapper()
  {
    CreateMap<CreateUserDto, User>();
    CreateMap<UpdateUserDto, User>();
    CreateMap<RegisterDto, User>();

    CreateMap<User, UserProfileDto>();
    CreateMap<User, UpdateUserDto>();
    CreateMap<User, HistoryDto>();
    
  }
}