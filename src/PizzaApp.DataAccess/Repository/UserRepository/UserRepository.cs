using Microsoft.EntityFrameworkCore;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Repository.UserRepository;

public class UserRepository(IDbContextFactory<PizzaAppDbContext> contextFactory)
    : Repository<UserEntity>(contextFactory), IUserRepository
{
    public async Task<UserEntity?> GetByIdWithDetailsAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.UserInfo)
            .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<UserEntity?> GetByGuidWithDetailsAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.UserInfo)
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.ExternalId == guid);
    }
    
    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<UserEntity?> GetByUserNameAsync(string userName)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }
    
    public async Task<UserEntity?> GetByPhoneNumberAsync(string phoneNumber)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }
    
    
    public async Task<UserEntity?> GetByIdWithUserInfoAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.UserInfo)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<UserEntity?> GetByIdWithRolesAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<UserEntity?> GetByIdWithOrdersAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.Orders)
                .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<UserEntity?> GetByIdWithAllDataAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.UserInfo)
            .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Orders)
                .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<UserEntity?> GetByGuidWithUserInfoAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.UserInfo)
            .FirstOrDefaultAsync(u => u.ExternalId == guid);
    }
    
    public async Task<UserEntity?> GetByGuidWithRolesAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.ExternalId == guid);
    }
    
    public async Task<UserEntity?> GetByGuidWithOrdersAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.Orders)
            .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(u => u.ExternalId == guid);
    }
    
    public async Task<UserEntity?> GetByGuidWithAllDataAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.UserInfo)
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .Include(u => u.Orders)
            .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(u => u.ExternalId == guid);
    }
    
    public async Task<List<UserEntity>> GetAllWithAllDataAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Users
            .Include(u => u.UserInfo)
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .Include(u => u.Orders)
            .ThenInclude(o => o.OrderItems)
            .ToListAsync();
    }

    
    
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .AnyAsync(u => u.Email == email);
    }
    
    public async Task<bool> ExistsByUserNameAsync(string userName)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .AnyAsync(u => u.UserName == userName);
    }
    
    public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .AnyAsync(u => u.PhoneNumber == phoneNumber);
    }
    
    
    public async Task<List<UserEntity>> GetUsersByRoleAsync(Role role)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.Roles.Any(ur => ur.Role.RoleType == role))
            .ToListAsync();
    }
    
    public async Task<List<UserEntity>> GetUsersByRoleIdAsync(int roleId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.Roles)
            .Where(u => u.Roles.Any(ur => ur.RoleId == roleId))
            .ToListAsync();
    }
    
    public async Task<List<UserEntity>> GetUsersByRoleGuidAsync(Guid roleGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.Roles.Any(ur => ur.Role.ExternalId == roleGuid))
            .ToListAsync();
    }
    
    
    public async Task<(List<UserEntity> Users, int TotalCount)> GetUsersPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var query = context.Users.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u => 
                u.UserName.Contains(searchTerm) ||
                u.Email.Contains(searchTerm) ||
                u.PhoneNumber.Contains(searchTerm));
        }
        
        query = sortBy?.ToLower() switch
        {
            "email" => ascending 
                ? query.OrderBy(u => u.Email) 
                : query.OrderByDescending(u => u.Email),
            "username" => ascending 
                ? query.OrderBy(u => u.UserName) 
                : query.OrderByDescending(u => u.UserName),
            "created" => ascending 
                ? query.OrderBy(u => u.CreationTime) 
                : query.OrderByDescending(u => u.CreationTime),
            _ => ascending 
                ? query.OrderBy(u => u.Id) 
                : query.OrderByDescending(u => u.Id)
        };
        
        var totalCount = await query.CountAsync();
        
        var users = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (users, totalCount);
    }
    
    
    public async Task<int> GetUserCountAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users.CountAsync();
    }
    
    public async Task<int> GetUsersWithOrdersCountAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Users
            .Where(u => u.Orders.Any())
            .CountAsync();
    }
    
    public async Task UpdateUserInfoAsync(int userId, bool isBlocked, string? blockInfo)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found");

        var userInfo = await context.UserInfos
            .FirstOrDefaultAsync(ui => ui.Id == user.UserInfoId);

        if (userInfo == null)
        {
            userInfo = new UserInfoEntity
            {
                ExternalId = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                IsBlocked = isBlocked,
                BlockInformation = blockInfo
            };

            user.UserInfo = userInfo;
        }
        else
        {
            userInfo.IsBlocked = isBlocked;
            userInfo.BlockInformation = blockInfo;
            userInfo.ModificationTime = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
    }
    public async Task UpdateUserInfoAsync(Guid userGuid, bool isBlocked, string? blockInfo)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.ExternalId == userGuid);

        if (user == null)
            throw new InvalidOperationException($"User with GUID {userGuid} not found");

        await UpdateUserInfoAsync(user.Id, isBlocked, blockInfo);
    }


    public async Task<List<RoleEntity>> GetAllRolesAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Roles.ToListAsync();
    }
    
    public async Task UpdateUserRolesAsync(int userId, List<int> newRoleIds)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var userExists = await context.Users
            .AnyAsync(u => u.Id == userId);

        if (!userExists)
            throw new InvalidOperationException($"User with ID {userId} not found");

        var newIds = newRoleIds.Distinct().ToHashSet();

        var existingRoleIds = await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        var toRemove = existingRoleIds.Except(newIds).ToList();
        var toAdd = newIds.Except(existingRoleIds).ToList();

        if (toRemove.Count > 0)
        {
            await context.UserRoles
                .Where(ur => ur.UserId == userId && toRemove.Contains(ur.RoleId))
                .ExecuteDeleteAsync();
        }

        if (toAdd.Count > 0)
        {
            var now = DateTime.UtcNow;

            context.UserRoles.AddRange(toAdd.Select(roleId => new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId,
                ExternalId = Guid.NewGuid(),
                CreationTime = now,
                ModificationTime = now
            }));
        }

        await context.SaveChangesAsync();
    }
    public async Task UpdateUserRolesAsync(Guid userGuid, List<Guid> newRoleGuids)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var user = await context.Users.Where(u => u.ExternalId == userGuid).FirstOrDefaultAsync();
        if (user == null)
            throw new InvalidOperationException($"User with GUID {userGuid} not found");
        
        var roleIds = await context.Roles
            .Where(r => newRoleGuids.Contains(r.ExternalId))
            .Select(r => r.Id)
            .ToListAsync();

        await UpdateUserRolesAsync(user.Id, roleIds);
    }
}