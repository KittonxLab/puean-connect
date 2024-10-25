using AutoMapper;
using PuanConnect.Dtos.Category;
using PuanConnect.Interfaces;

namespace PuanConnect.Services;

public class CategoryService : ICategoryService
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly IMapper _mapper;

  public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
  {
    _categoryRepository = categoryRepository;
    _mapper = mapper;
  }

  public async Task<List<CategoryDto>> GetAllCategories()
  {
    var categories = await _categoryRepository.GetAllCategories();

    return _mapper.Map<List<CategoryDto>>(categories);
  }
}