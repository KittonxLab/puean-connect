using PuanConnect.Dtos.Category;

namespace PuanConnect.Interfaces;

public interface ICategoryService
{
  Task<List<CategoryDto>> GetAllCategories();
}