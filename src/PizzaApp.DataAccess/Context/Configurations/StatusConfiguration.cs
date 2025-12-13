using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<StatusEntity>
{
    public void Configure(EntityTypeBuilder<StatusEntity> e)
    {
        e.HasKey(s => s.Id);
        e.Property(s => s.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        e.HasIndex(s => s.ExternalId).IsUnique();
        e.HasIndex(s => s.Name).IsUnique();
        e.Property(s => s.Name);
    }
}