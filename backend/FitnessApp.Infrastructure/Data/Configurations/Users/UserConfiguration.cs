using System;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .IsRequired() 
            .HasMaxLength(100);
            
         builder.Property(u => u.LastName) 
            .HasColumnName("last_name") 
            .IsRequired() 
            .HasMaxLength(10); 
        
        builder.Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(100); 
            
        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash") 
            .IsRequired() 
            .HasMaxLength(255); 
        
        builder.Property(u => u.PhoneNumber) 
            .HasColumnName("phone_number") 
            .HasMaxLength(20)
            .IsRequired(false);
            
        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(u => u.IsActive) 
            .HasColumnName("is_active") 
            .IsRequired()
            .HasDefaultValue(true);
         

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .IsRequired();
            

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate()
            .IsRequired();
            
        builder.HasOne(u => u.ClientProfile) 
            .WithOne(c => c.User)
            .HasForeignKey<Client>(c => c.UserId) 
            .OnDelete(DeleteBehavior.Cascade); 
            
        builder.HasOne(u => u.TrainerProfile) 
            .WithOne(t => t.User)
            .HasForeignKey<Trainer>(t => t.UserId) 
            .OnDelete(DeleteBehavior.Cascade);
    }
}
