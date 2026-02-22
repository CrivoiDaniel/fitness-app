using FitnessApp.Domain.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class BenefitConfiguration : IEntityTypeConfiguration<Benefit>
{
    public void Configure(EntityTypeBuilder<Benefit> builder)
    {
        // Table name
        builder.ToTable("benefits");

        // Primary Key
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");

        // Properties
        builder.Property(b => b.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.Description)
            .HasColumnName("description")
            .HasColumnType("TEXT");

        builder.Property(b => b.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)") 
            .IsRequired();

        builder.Property(b => b.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)") 
            .ValueGeneratedOnAddOrUpdate()
            .IsRequired();


        // Indexes
        builder.HasIndex(b => b.Name)
            .IsUnique()
            .HasDatabaseName("idx_benefit_name");

        builder.HasIndex(b => b.DisplayName)
            .IsUnique()
            .HasDatabaseName("idx_benefit_display_name");

        builder.HasIndex(b => b.IsActive)
            .HasDatabaseName("idx_benefit_is_active");

        // Relationships
        builder.HasMany(b => b.BenefitPackageItems)
            .WithOne(bpi => bpi.Benefit)
            .HasForeignKey(bpi => bpi.BenefitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}