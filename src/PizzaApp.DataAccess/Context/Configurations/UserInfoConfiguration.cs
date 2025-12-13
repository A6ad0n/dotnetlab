using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfoEntity>
{
    public void Configure(EntityTypeBuilder<UserInfoEntity> e)
    {
        e.HasKey(ui => ui.Id);
        e.Property(ui => ui.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        e.HasIndex(ui => ui.ExternalId).IsUnique();
        e.Property(ui => ui.BlockInformation).HasMaxLength(255);
    }
}