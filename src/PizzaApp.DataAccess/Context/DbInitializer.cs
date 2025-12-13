using Microsoft.EntityFrameworkCore;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Context;

public static class SeedConstants
{
    public static readonly DateTime SeedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    public static class Guids
    {
        public static readonly Guid RoleAnonymous = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid RoleAdmin = Guid.Parse("00000000-0000-0000-0000-000000000002");
        public static readonly Guid RoleCustomer = Guid.Parse("00000000-0000-0000-0000-000000000003");
        public static readonly Guid RoleDeveloper = Guid.Parse("00000000-0000-0000-0000-000000000004");
        
        public static readonly Guid UserAdmin = Guid.Parse("00000000-0000-0000-0000-000000000101");
        
        public static readonly Guid UserRoleAdmin = Guid.Parse("00000000-0000-0000-0000-000000000201");
    }
    
    public static class SecurityStamps
    {
        public const string Admin = "AQAAAAIAAYagAAAAEFagX+6l19G70fvBcw9DR0KRwJcEt1wyZoHIHjMdEGZqlVy6PL6w2aHTf8stGpZodw==";
    }
}

public static class DbInitializer
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        SeedRoles(modelBuilder);
        SeedUsers(modelBuilder);
        SeedUserRoles(modelBuilder);
    }
    public static void SeedRoles(ModelBuilder modelBuilder)
    {
        var roles = new List<RoleEntity>
        {
            new RoleEntity
            {
                Id = 1,
                ExternalId = SeedConstants.Guids.RoleAnonymous,
                RoleType = Role.Anonymous,
                Name = Role.Anonymous.ToString(),
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new RoleEntity
            {
                Id = 2,
                ExternalId = SeedConstants.Guids.RoleAdmin,
                Name = Role.Admin.ToString(),
                RoleType = Role.Admin,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new RoleEntity
            {
                Id = 3,
                ExternalId = SeedConstants.Guids.RoleCustomer,
                Name = Role.Customer.ToString(),
                RoleType = Role.Customer,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new RoleEntity
            {
                Id = 4,
                ExternalId = SeedConstants.Guids.RoleDeveloper,
                Name = Role.Developer.ToString(),
                RoleType = Role.Developer,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            }
        };
        
        modelBuilder.Entity<RoleEntity>().HasData(roles);
    }

    public static void SeedUsers(ModelBuilder modelBuilder)
    {
        var users = new List<UserEntity>
        {
            new UserEntity
            {
                Id = 1,
                ExternalId = SeedConstants.Guids.UserAdmin,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.ru",
                NormalizedEmail = "ADMIN@ADMIN.RU",
                PhoneNumber = "77777777777",
                PasswordHash = SeedConstants.SecurityStamps.Admin,
                SecurityStamp = "5A9B8C7D-6E5F-4A3B-2C1D-0E9F8A7B6C5D",
                ConcurrencyStamp = "1A2B3C4D-5E6F-7A8B-9C0D-1E2F3A4B5C6D"
            }
        };
        
        modelBuilder.Entity<UserEntity>().HasData(users);
    }

    public static void SeedUserRoles(ModelBuilder modelBuilder)
    {
        var userRoles = new List<UserRoleEntity>
        {
            new UserRoleEntity
            {
                Id = 1,
                ExternalId =  SeedConstants.Guids.UserRoleAdmin,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate,
                RoleId = 2,
                UserId = 1
            }
        };
        modelBuilder.Entity<UserRoleEntity>().HasData(userRoles);
    }
}