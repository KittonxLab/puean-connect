using PuanConnect.Models;

namespace PuanConnect.Interfaces;

public interface ICategoryRepository
{
  Task<List<Category>> GetAllCategories();
}