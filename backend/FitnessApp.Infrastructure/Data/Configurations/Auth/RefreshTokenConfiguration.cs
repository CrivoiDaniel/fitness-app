using System;
using FitnessApp.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations.Auth;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(rt => rt.Id);
        
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(rt => rt.CreatedByIp)
            .HasMaxLength(50);
        
        builder.Property(rt => rt.RevokedByIp)
            .HasMaxLength(50);
        
        builder.Property(rt => rt.ReplacedByToken)
            .HasMaxLength(500);
        
        // Index pentru căutare rapidă după token
        builder.HasIndex(rt => rt.Token)
            .IsUnique();
        
        // Index pentru găsire tokeni activi ai unui user
        builder.HasIndex(rt => new { rt.UserId, rt.IsRevoked, rt.ExpiresAt });
        
        // Relationship cu User
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}