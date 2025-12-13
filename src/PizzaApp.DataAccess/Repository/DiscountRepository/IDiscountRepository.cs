using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Repository.DiscountRepository;

public interface IDiscountRepository : IRepository<DiscountEntity>
{
    Task<DiscountEntity?> GetByIdWithDetailsAsync(int id);
    Task<DiscountEntity?> GetByGuidWithDetailsAsync(Guid guid);
    
    Task<DiscountEntity?> GetByIdWithStatusAsync(int id);
    Task<DiscountEntity?> GetByGuidWithStatusAsync(Guid guid);
    
    Task<List<DiscountEntity>> GetAllWithDetailsAsync();
    
    Task<List<DiscountEntity>> GetDiscountsByStatusAsync(Status status);
    Task<List<DiscountEntity>> GetDiscountsByStatusIdAsync(int statusId);
    Task<List<DiscountEntity>> GetDiscountsByStatusGuidAsync(Guid statusGuid);
    
    Task<(List<DiscountEntity> Discounts, int TotalCount)> GetDiscountsPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true);
    
    Task<int> GetDiscountsCountAsync();
    
    Task<bool> ExistsStatusAsync(int statusId);
    Task<bool> ExistsStatusAsync(Guid statusGuid);
    Task<List<StatusEntity>> GetAllStatusesAsync();
    Task UpdateDiscountStatusAsync(DiscountEntity discount, int statusId);
    Task UpdateDiscountStatusAsync(DiscountEntity discount, Guid statusGuid);
}