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
        
        public static readonly Guid C1 = Guid.Parse("00000000-0000-0000-0000-000000000011");
        public static readonly Guid C2 = Guid.Parse("00000000-0000-0000-0000-000000000022");
        public static readonly Guid C3 = Guid.Parse("00000000-0000-0000-0000-000000000033");
        public static readonly Guid C4 = Guid.Parse("00000000-0000-0000-0000-000000000044");
        public static readonly Guid C5 = Guid.Parse("00000000-0000-0000-0000-000000000055");
        public static readonly Guid C6 = Guid.Parse("00000000-0000-0000-0000-000000000066");
        public static readonly Guid C7 = Guid.Parse("00000000-0000-0000-0000-000000000077");
        
        public static readonly Guid S1 = Guid.Parse("00000000-0000-0000-0000-000000000111");
        public static readonly Guid S2 = Guid.Parse("00000000-0000-0000-0000-000000000222");
        public static readonly Guid S3 = Guid.Parse("00000000-0000-0000-0000-000000000333");
        public static readonly Guid S4 = Guid.Parse("00000000-0000-0000-0000-000000000444");
        public static readonly Guid S5 = Guid.Parse("00000000-0000-0000-0000-000000000555");
        public static readonly Guid S6 = Guid.Parse("00000000-0000-0000-0000-000000000666");
        public static readonly Guid S7 = Guid.Parse("00000000-0000-0000-0000-000000000777");
        public static readonly Guid S8 = Guid.Parse("00000000-0000-0000-0000-000000000888");
        public static readonly Guid S9 = Guid.Parse("00000000-0000-0000-0000-000000000999");
        public static readonly Guid S10 = Guid.Parse("00000000-0000-0000-0000-000000001111");
        public static readonly Guid S11 = Guid.Parse("00000000-0000-0000-0000-000000002222");
        public static readonly Guid S12 = Guid.Parse("00000000-0000-0000-0000-000000003333");
        public static readonly Guid S13 = Guid.Parse("00000000-0000-0000-0000-000000004444");
        
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

    private static void SeedRoles(ModelBuilder modelBuilder)
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

    private static void SeedUsers(ModelBuilder modelBuilder)
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

    private static void SeedUserRoles(ModelBuilder modelBuilder)
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

    private static void SeedCategories(ModelBuilder modelBuilder)
    {
        var categories = new List<MenuCategoryEntity>
        {
            new MenuCategoryEntity
            {
                Id = 1,
                ExternalId = SeedConstants.Guids.C1,
                Name = MenuCategory.Pizza,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new MenuCategoryEntity
            {
                Id = 2,
                ExternalId = SeedConstants.Guids.C2,
                Name = MenuCategory.Drink,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new MenuCategoryEntity
            {
                Id = 3,
                ExternalId = SeedConstants.Guids.C3,
                Name = MenuCategory.Sauce,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new MenuCategoryEntity
            {
                Id = 4,
                ExternalId = SeedConstants.Guids.C4,
                Name = MenuCategory.Dessert,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new MenuCategoryEntity
            {
                Id = 5,
                ExternalId = SeedConstants.Guids.C5,
                Name = MenuCategory.Snack,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new MenuCategoryEntity
            {
                Id = 6,
                ExternalId = SeedConstants.Guids.C6,
                Name = MenuCategory.Combo,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new MenuCategoryEntity
            {
                Id = 7,
                ExternalId = SeedConstants.Guids.C7,
                Name = MenuCategory.SpecialOffer,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            }
        };
        
        modelBuilder.Entity<MenuCategoryEntity>().HasData(categories);
    }

    private static void SeedStatuses(ModelBuilder modelBuilder)
    {
        var statuses = new List<StatusEntity>
        {
            new StatusEntity
            {
                Id = 1,
                ExternalId = SeedConstants.Guids.S1,
                Name = Status.OrderCreated,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 2,
                ExternalId = SeedConstants.Guids.S2,
                Name = Status.OrderConfirmed,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 3,
                ExternalId = SeedConstants.Guids.S3,
                Name = Status.OrderPreparing,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 4,
                ExternalId = SeedConstants.Guids.S4,
                Name = Status.OrderReady,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 5,
                ExternalId = SeedConstants.Guids.S5,
                Name = Status.OrderDelivering,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 6,
                ExternalId = SeedConstants.Guids.S6,
                Name = Status.OrderCompleted,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 7,
                ExternalId = SeedConstants.Guids.S7,
                Name = Status.OrderCancelled,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 8,
                ExternalId = SeedConstants.Guids.S8,
                Name = Status.MenuActive,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 9,
                ExternalId = SeedConstants.Guids.S9,
                Name = Status.MenuInactive,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 10,
                ExternalId = SeedConstants.Guids.S10,
                Name = Status.MenuArchived,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 11,
                ExternalId = SeedConstants.Guids.S11,
                Name = Status.DiscountActive,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 12,
                ExternalId = SeedConstants.Guids.S12,
                Name = Status.DiscountExpired,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            },
            new StatusEntity
            {
                Id = 13,
                ExternalId = SeedConstants.Guids.S13,
                Name = Status.DiscountScheduled,
                CreationTime = SeedConstants.SeedDate,
                ModificationTime = SeedConstants.SeedDate
            }
        };
        
        modelBuilder.Entity<StatusEntity>().HasData(statuses);
    }
}