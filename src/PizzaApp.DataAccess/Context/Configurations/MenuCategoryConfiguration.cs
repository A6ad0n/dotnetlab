using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategoryEntity>
{
    public void Configure(EntityTypeBuilder<MenuCategoryEntity> e)
    {
        e.HasKey(mc => mc.Id);
        e.Property(mc => mc.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        e.HasIndex(mc => mc.ExternalId).IsUnique();
        e.HasIndex(mc => mc.Name).IsUnique();
        e.Property(mc => mc.Name);
    }
}