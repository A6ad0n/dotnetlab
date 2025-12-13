using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<DiscountEntity>
{
    public void Configure(EntityTypeBuilder<DiscountEntity> e)
    {
        e.HasKey(d => d.Id);
        e.Property(d => d.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        e.HasIndex(d => d.ExternalId).IsUnique();
        e.Property(d => d.Name).HasMaxLength(50);

        e.ToTable(t => t.HasCheckConstraint("CH_discounts_valid_dates", "\"ValidFrom\" < \"ValidTo\""));
        e.ToTable(t => t.HasCheckConstraint("CH_discounts_discount_percentage", "\"DiscountPercentage\" >= 0 AND \"DiscountPercentage\" <= 100"));

        e.HasOne(d => d.Status)
            .WithMany(s => s.Discounts)
            .HasForeignKey(d => d.StatusId);
    }
}