using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> e)
    {
        e.HasKey(o => o.Id);
        e.HasIndex(o => o.ExternalId).IsUnique();
        e.Property(o => o.UpdatedAt).IsRequired(false);
        e.Property(o => o.StatusChangedAt).IsRequired(false);

        e.ToTable(t => t.HasCheckConstraint("CH_orders_valid_dates", "\"UpdatedAt\" >= \"CreatedAt\" AND \"StatusChangedAt\" >= \"CreatedAt\""));
        e.ToTable(t => t.HasCheckConstraint("CH_orders_total_price", "\"TotalPrice\" >= 0"));

        e.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);

        e.HasOne(o => o.Status)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.StatusId);
    }
}