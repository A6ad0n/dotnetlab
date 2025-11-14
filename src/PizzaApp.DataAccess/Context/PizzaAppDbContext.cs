using PizzaApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace PizzaApp.DataAccess.Context;

public class PizzaAppDbContext : DbContext
{
    public DbSet<StatusEntity> Statuses { get; set; }
    
    public DbSet<MenuCategoryEntity> MenuCategories { get; set; }
    public DbSet<MenuItemEntity> MenuItems { get; set; }
    public DbSet<DiscountEntity> Discounts { get; set; }
    public DbSet<MenuItemDiscountEntity> MenuItemDiscounts { get; set; }
    
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    
    public DbSet<UserInfoEntity> UserInfos { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    
    public PizzaAppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PizzaAppDbContext).Assembly);
    }
}