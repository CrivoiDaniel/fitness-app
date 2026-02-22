using System;
using FitnessApp.Domain.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class BenefitPackageItemConfiguration : IEntityTypeConfiguration<BenefitPackageItem>
{
    public void Configure(EntityTypeBuilder<BenefitPackageItem> builder)
    {
        // Table name
        builder.ToTable("benefit_package_items");

        // Primary Key
        builder.HasKey(bpi => bpi.Id);
        builder.Property(bpi => bpi.Id).HasColumnName("id");

        // Properties
        builder.Property(bpi => bpi.BenefitPackageId)
            .HasColumnName("benefit_package_id")
            .IsRequired();

        builder.Property(bpi => bpi.BenefitId)
            .HasColumnName("benefit_id")
            .IsRequired();

        builder.Property(bpi => bpi.Value)
            .HasColumnName("value")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(bpi => bpi.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)") 
            .IsRequired();

        builder.Property(bpi => bpi.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)") 
            .ValueGeneratedOnAddOrUpdate()
            .IsRequired();

        // Indexes
        builder.HasIndex(bpi => new { bpi.BenefitPackageId, bpi.BenefitId })
            .IsUnique()
            .HasDatabaseName("idx_benefit_package_item_unique");

        builder.HasIndex(bpi => bpi.BenefitPackageId)
            .HasDatabaseName("idx_benefit_package_item_package");

        builder.HasIndex(bpi => bpi.BenefitId)
            .HasDatabaseName("idx_benefit_package_item_benefit");

        // Relationships are defined in Benefit and BenefitPackage configurations
    }

}
