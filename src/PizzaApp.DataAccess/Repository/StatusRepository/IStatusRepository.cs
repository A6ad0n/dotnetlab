using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Repository.StatusRepository;

public interface IStatusRepository : IRepository<StatusEntity>
{
    Task<StatusEntity?> GetByIdWithDetailsAsync(int id);
    Task<StatusEntity?> GetByGuidWithDetailsAsync(Guid guid);
    
    Task<StatusEntity?> GetByIdWithDiscountsAsync(int id);
    Task<StatusEntity?> GetByIdWithMenuItemsAsync(int id);
    Task<StatusEntity?> GetByIdWithOrdersAsync(int id);
    Task<StatusEntity?> GetByIdWithAllDataAsync(int id);
    
    Task<StatusEntity?> GetByGuidWithDiscountsAsync(Guid guid);
    Task<StatusEntity?> GetByGuidWithMenuItemsAsync(Guid guid);
    Task<StatusEntity?> GetByGuidWithOrdersAsync(Guid guid);
    Task<StatusEntity?> GetByGuidWithAllDataAsync(Guid guid);
    
    Task<List<StatusEntity>> GetAllWithDetailsAsync();
    
    Task<(List<StatusEntity> Statuses, int TotalCount)> GetStatusesPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true);
    
    Task<int> GetStatusesCountAsync();
}