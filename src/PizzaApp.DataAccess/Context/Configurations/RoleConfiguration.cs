using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> e)
    {
        e.HasKey(r => r.Id);
        e.Property(r => r.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        e.HasIndex(r => r.ExternalId).IsUnique();
        e.HasIndex(r => r.Name).IsUnique();
        e.Property(r => r.Name);
    }
}