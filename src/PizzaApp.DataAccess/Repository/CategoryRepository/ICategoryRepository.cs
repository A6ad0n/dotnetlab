using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Repository.CategoryRepository;

public interface ICategoryRepository : IRepository<MenuCategoryEntity>
{
    Task<MenuCategoryEntity?> GetByIdWithDetailsAsync(int id);
    Task<MenuCategoryEntity?> GetByGuidWithDetailsAsync(Guid guid);
    
    Task<MenuCategoryEntity?> GetByIdWithAllDataAsync(int id);
    Task<MenuCategoryEntity?> GetByGuidWithAllDataAsync(Guid guid);
    
    Task<List<MenuCategoryEntity>> GetAllWithDetailsAsync();
    
    Task<(List<MenuCategoryEntity> Categories, int TotalCount)> GetCategoriesPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true);
    
    Task<int> GetCategoriesCountAsync();
}