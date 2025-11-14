using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> e)
    {
        e.HasKey(u => u.Id);
        e.HasIndex(u => u.ExternalId).IsUnique();
        e.Property(u => u.Username).HasMaxLength(255);
        e.Property(u => u.Email).HasMaxLength(255).IsRequired(false);
        e.Property(u => u.Phone).HasMaxLength(20);
        e.HasIndex(u => u.Username).IsUnique();
        e.HasIndex(u => u.Email).IsUnique();
        e.HasIndex(u => u.Phone).IsUnique();

        e.ToTable(t => t.HasCheckConstraint("CH_users_valid_email", "\"Email\" ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\\\.[A-Za-z]{2,}$'"));
        e.ToTable(t => t.HasCheckConstraint("CH_users_valid_phone", "\"Phone\" ~ '^[+]?[0-9\\\\-\\\\(\\\\)\\\\s]{10,15}$'"));

        e.HasOne(u => u.UserInfo)
            .WithOne(ui => ui.User)
            .HasForeignKey<UserEntity>(u => u.UserInfoId);
    }
}