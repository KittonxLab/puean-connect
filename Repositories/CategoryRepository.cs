using Microsoft.EntityFrameworkCore;
using PuanConnect.Database;
using PuanConnect.Interfaces;
using PuanConnect.Models;

public class CategoryRepository : ICategoryRepository
{
  private readonly AppDBContext _dbContext;

  public CategoryRepository(AppDBContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<List<Category>> GetAllCategories()
  {
    return await _dbContext.Categories.ToListAsync();
  }
}