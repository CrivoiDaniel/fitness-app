using System;
using FitnessApp.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class ClientConfiguration: IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder) 
    { 
        builder.ToTable("clients");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id) 
            .HasColumnName("id") 
            .ValueGeneratedOnAdd(); 
        
        builder.Property(c => c.UserId) 
            .HasColumnName("user_id") 
            .IsRequired(); 
        
        builder.Property(c => c.DateOfBirth) 
            .HasColumnName("date_of_birth") 
            .HasColumnType("date") 
            .IsRequired(); 
        
        // {/* Configure relationship with User */} 
        builder.HasOne(c => c.user) 
            .WithOne(u => u.ClientProfile) 
            .HasForeignKey<Client>(c => c.UserId) 
            .OnDelete(DeleteBehavior.Cascade); 
    }

}
