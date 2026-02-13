using System;
using System.Security.Cryptography;
using FitnessApp.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
   public void Configure(EntityTypeBuilder<Trainer> builder)
    { 
        builder.ToTable("trainers");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id) 
            .HasColumnName("id") 
            .ValueGeneratedOnAdd(); 
        
        builder.Property(t => t.UserId) 
            .HasColumnName("user_id") 
            .IsRequired(); 
            
        builder.Property(t => t.Specialization) 
            .HasColumnName("specialization")
            .IsRequired() 
            .HasMaxLength(100); 

        builder.Property(t => t.YearsOfExperience) 
            .HasColumnName("years_of_experience") 
            .IsRequired(); 
        
        builder.Property(t => t.Rating) 
            .HasColumnName("rating") 
            .HasColumnType("decimal(3,2)") 
            .IsRequired() 
            .HasDefaultValue(5.0m); 

        // {/* Configure relationship with User */}

        builder.HasOne(t => t.user)
            .WithOne(u => u.TrainerProfile) 
            .HasForeignKey<Trainer>(t => t.UserId) 
            .OnDelete(DeleteBehavior.Cascade); 
    }  
}
