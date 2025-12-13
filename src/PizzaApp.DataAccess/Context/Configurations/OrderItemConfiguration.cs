using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> e)
    {
        e.HasKey(oi => oi.Id);
        e.Property(oi => oi.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        e.HasIndex(oi => oi.ExternalId).IsUnique();

        e.ToTable(t => t.HasCheckConstraint("CH_order_items_unit_price", "\"UnitPrice\" >= 0"));
        e.ToTable(t => t.HasCheckConstraint("CH_order_items_quantity", "\"Quantity\" >= 0"));
        e.ToTable(t => t.HasCheckConstraint("CH_order_items_discount_applied", "\"DiscountApplied\" >= 0 AND \"DiscountApplied\" <= \"Quantity\" * \"UnitPrice\""));

        e.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        e.HasOne(oi => oi.MenuItem)
            .WithMany(mi => mi.OrderItems)
            .HasForeignKey(oi => oi.MenuItemId);
    }
}