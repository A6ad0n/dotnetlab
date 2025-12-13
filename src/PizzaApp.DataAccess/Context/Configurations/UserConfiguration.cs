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
        e.Property(u => u.UserName).HasMaxLength(255);
        e.Property(u => u.Email).HasMaxLength(255).IsRequired(false);
        e.Property(u => u.PhoneNumber).HasMaxLength(20);
        e.HasIndex(u => u.UserName).IsUnique();
        e.HasIndex(u => u.Email).IsUnique();
        e.HasIndex(u => u.PhoneNumber).IsUnique();

        // e.ToTable(t => t.HasCheckConstraint("CH_users_valid_email", "\"Email\" ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\\\.[A-Za-z]{2,}$'"));
        e.ToTable(t => t.HasCheckConstraint("CH_users_valid_phone", "\"PhoneNumber\" ~ '^(\\+?\\d{11}|\\d{10})$'"));

        e.Property(u => u.UserInfoId).IsRequired(false);

        e.HasOne(u => u.UserInfo)
            .WithOne(ui => ui.User)
            .HasForeignKey<UserEntity>(u => u.UserInfoId);
    }
}