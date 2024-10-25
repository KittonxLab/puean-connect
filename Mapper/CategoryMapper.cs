using AutoMapper;
using PuanConnect.Dtos.Category;
using PuanConnect.Models;

namespace PuanConnect.Mapper;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<Category, CategoryDto>();
    }
}