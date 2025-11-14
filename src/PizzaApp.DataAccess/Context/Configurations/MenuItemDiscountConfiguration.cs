using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class MenuItemDiscountConfiguration : IEntityTypeConfiguration<MenuItemDiscountEntity>
{
    public void Configure(EntityTypeBuilder<MenuItemDiscountEntity> e)
    {
        e.HasKey(mid => mid.Id);
        e.HasIndex(mid => mid.ExternalId).IsUnique();

        e.HasOne(mid => mid.MenuItem)
            .WithMany(mi => mi.Discounts)
            .HasForeignKey(mid => mid.MenuItemId);

        e.HasOne(mid => mid.Discount)
            .WithMany(d => d.MenuItems)
            .HasForeignKey(mid => mid.DiscountId);
    }
}