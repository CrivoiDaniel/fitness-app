using FitnessApp.Domain.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class BenefitPackageConfiguration : IEntityTypeConfiguration<BenefitPackage>
{
    public void Configure(EntityTypeBuilder<BenefitPackage> builder)
    {
        // Table name
        builder.ToTable("benefit_packages");

        // Primary Key
        builder.HasKey(bp => bp.Id);
        builder.Property(bp => bp.Id).HasColumnName("id");

        // Properties
        builder.Property(bp => bp.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(bp => bp.ScheduleWeekday)
            .HasColumnName("schedule_weekday")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(bp => bp.ScheduleWeekend)
            .HasColumnName("schedule_weekend")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(bp => bp.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(bp => bp.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(bp => bp.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Indexes
        builder.HasIndex(bp => bp.Name)
            .IsUnique()
            .HasDatabaseName("idx_benefit_package_name");

        builder.HasIndex(bp => bp.IsActive)
            .HasDatabaseName("idx_benefit_package_is_active");

        // Relationships
        builder.HasMany(bp => bp.BenefitPackageItems)
            .WithOne(bpi => bpi.BenefitPackage)
            .HasForeignKey(bpi => bpi.BenefitPackageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(bp => bp.SubscriptionPlans)
            .WithOne(sp => sp.BenefitPackage)
            .HasForeignKey(sp => sp.BenefitPackageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}