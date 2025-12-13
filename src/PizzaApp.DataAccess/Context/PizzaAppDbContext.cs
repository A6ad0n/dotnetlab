using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PizzaApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace PizzaApp.DataAccess.Context;

public class PizzaAppDbContext(DbContextOptions options) : IdentityDbContext<
    UserEntity,
    RoleEntity,
    int,
    IdentityUserClaim<int>,
    UserRoleEntity,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>>(options)
{
    public DbSet<StatusEntity> Statuses { get; set; }
    
    public DbSet<MenuCategoryEntity> MenuCategories { get; set; }
    public DbSet<MenuItemEntity> MenuItems { get; set; }
    public DbSet<DiscountEntity> Discounts { get; set; }
    public DbSet<MenuItemDiscountEntity> MenuItemDiscounts { get; set; }
    
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    
    public DbSet<UserInfoEntity> UserInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PizzaAppDbContext).Assembly);
        DbInitializer.SeedData(modelBuilder);
    }
}