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
    
    public async Task UpdateUserInfoAsync(UserEntity user, bool isBlocked, string? blockInfo)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        UserInfoEntity userInfo;
        if (user.UserInfo != null)
        {
            userInfo = user.UserInfo;
            userInfo.ModificationTime = DateTime.UtcNow;
        }
        else
        {
            userInfo = new UserInfoEntity
            {
                ExternalId = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
            };
            await context.UserInfos.AddAsync(userInfo);
            await context.SaveChangesAsync();
            user.UserInfo = userInfo;
            user.UserInfoId = user.UserInfo.Id;
        }

        userInfo.IsBlocked = isBlocked;
        userInfo.BlockInformation = blockInfo;

        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    
    public async Task<List<RoleEntity>> GetAllRolesAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Roles.ToListAsync();
    }
    
    public async Task UpdateUserRolesAsync(int userId, List<int> newRoleIds)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var existingRoles = await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .ToListAsync();
        
        if (existingRoles.Any())
            context.UserRoles.RemoveRange(existingRoles);
        
        var rolesToAdd = newRoleIds.Select(id => new UserRoleEntity
        {
            UserId = userId,
            RoleId = id,
            CreationTime = DateTime.UtcNow,
            ModificationTime = DateTime.UtcNow,
            ExternalId = Guid.NewGuid()
        }).ToList();

        if (rolesToAdd.Count != 0)
            context.UserRoles.AddRange(rolesToAdd);

        await context.SaveChangesAsync();
    }




   private async Task<UserEntity> SaveWithDetailsAsync(UserEntity user)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        if (user.UserInfo != null)
        {
            if (user.UserInfo.Id > 0)
            {
                user.UserInfo.ModificationTime = DateTime.UtcNow;
                context.UserInfos.Update(user.UserInfo);
            }
            else
            {
                user.UserInfo.CreationTime = DateTime.UtcNow;
                user.UserInfo.ModificationTime = DateTime.UtcNow;
                await context.UserInfos.AddAsync(user.UserInfo);
            }
        }

        if (user.Id > 0)
        {
            user.ModificationTime = DateTime.UtcNow;
            context.Users.Update(user);
        }
        else
        {
            user.ExternalId = Guid.NewGuid();
            user.CreationTime = DateTime.UtcNow;
            user.ModificationTime = user.CreationTime;
            await context.Users.AddAsync(user);
        }
        
        if (user.Roles != null)
        {
            var existingRoles = await context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .ToListAsync();

            var newRoleIds = user.Roles.Select(r => r.RoleId).Distinct().ToList();
            var existingRoleIds = existingRoles.Select(er => er.RoleId).ToHashSet();
            
            var rolesToRemove = existingRoles
                .Where(er => !newRoleIds.Contains(er.RoleId))
                .ToList();
            if (rolesToRemove.Any())
                context.UserRoles.RemoveRange(rolesToRemove);
            
            var rolesToAdd = newRoleIds
                .Where(rid => !existingRoleIds.Contains(rid))
                .Select(rid => new UserRoleEntity
                {
                    UserId = user.Id,
                    RoleId = rid,
                    CreationTime = DateTime.UtcNow,
                    ModificationTime = DateTime.UtcNow,
                    ExternalId = Guid.NewGuid()
                })
                .ToList();

            if (rolesToAdd.Count != 0)
                context.UserRoles.AddRange(rolesToAdd);
        }


        await context.SaveChangesAsync();
        if (user.UserInfo != null)
            user.UserInfoId = user.UserInfo.Id;
        user.Roles = await context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Include(ur => ur.Role)
            .ToListAsync();
        return user;
    }

}