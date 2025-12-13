using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItemEntity>
{
    public void Configure(EntityTypeBuilder<MenuItemEntity> e)
    {
        e.HasKey(mi => mi.Id);
        e.Property(mi => mi.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        e.HasIndex(mi => mi.ExternalId).IsUnique();
        e.Property(mi => mi.Name).HasMaxLength(100);
        e.ToTable(t => t.HasCheckConstraint("CH_menu_item_price", "\"Price\" >= 0"));

        e.HasOne(mi => mi.Category)
            .WithMany(c => c.MenuItems)
            .HasForeignKey(mi => mi.CategoryId);

        e.HasOne(mi => mi.Status)
            .WithMany(s => s.MenuItems)
            .HasForeignKey(mi => mi.StatusId);
    }
}
