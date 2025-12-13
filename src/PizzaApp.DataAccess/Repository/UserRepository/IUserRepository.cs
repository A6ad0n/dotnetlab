using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Repository.UserRepository;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity?> GetByIdWithDetailsAsync(int id);
    Task<UserEntity?> GetByGuidWithDetailsAsync(Guid guid);
    Task<UserEntity?> GetByEmailAsync(string email);
    Task<UserEntity?> GetByUserNameAsync(string userName);
    Task<UserEntity?> GetByPhoneNumberAsync(string phoneNumber);
    
    Task<UserEntity?> GetByIdWithUserInfoAsync(int id);
    Task<UserEntity?> GetByIdWithRolesAsync(int id);
    Task<UserEntity?> GetByIdWithOrdersAsync(int id);
    Task<UserEntity?> GetByIdWithAllDataAsync(int id);
    Task<UserEntity?> GetByGuidWithUserInfoAsync(Guid guid);
    Task<UserEntity?> GetByGuidWithRolesAsync(Guid guid);
    Task<UserEntity?> GetByGuidWithOrdersAsync(Guid guid);
    Task<UserEntity?> GetByGuidWithAllDataAsync(Guid guid);
    Task<List<UserEntity>> GetAllWithAllDataAsync();
    
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByUserNameAsync(string userName);
    Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
    
    Task<List<UserEntity>> GetUsersByRoleAsync(Role role);
    Task<List<UserEntity>> GetUsersByRoleIdAsync(int roleId);
    Task<List<UserEntity>> GetUsersByRoleGuidAsync(Guid roleGuid);
    
    Task<(List<UserEntity> Users, int TotalCount)> GetUsersPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true);
    
    Task<int> GetUserCountAsync();
    Task<int> GetUsersWithOrdersCountAsync();

    Task<List<RoleEntity>> GetAllRolesAsync();
    Task UpdateUserRolesAsync(UserEntity user, List<int> newRoleIds);
    Task UpdateUserRolesAsync(UserEntity user, List<Guid> newRoleGuids);

    Task UpdateUserInfoAsync(UserEntity user, bool isBlocked, string? blockInfo);
}