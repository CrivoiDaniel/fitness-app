using FitnessApp.Domain.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        // Table name
        builder.ToTable("subscription_plans");

        // Primary Key
        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Id).HasColumnName("id");

        // Properties
        builder.Property(sp => sp.Type)
            .HasColumnName("type")
            .HasMaxLength(50)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(sp => sp.DurationInMonths)
            .HasColumnName("duration_months")
            .IsRequired();

        builder.Property(sp => sp.Price)
            .HasColumnName("price")
            .HasColumnType("DECIMAL(10,2)")
            .IsRequired();

        builder.Property(sp => sp.BenefitPackageId)
            .HasColumnName("benefit_package_id")
            .IsRequired();

        builder.Property(sp => sp.IsRecurring)
            .HasColumnName("is_recurring")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(sp => sp.AllowInstallments)
            .HasColumnName("allow_installments")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(sp => sp.MaxInstallments)
            .HasColumnName("max_installments")
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(sp => sp.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(sp => sp.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(sp => sp.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Indexes
        builder.HasIndex(sp => sp.Type)
            .HasDatabaseName("idx_subscription_plan_type");

        builder.HasIndex(sp => sp.IsActive)
            .HasDatabaseName("idx_subscription_plan_is_active");

        builder.HasIndex(sp => sp.BenefitPackageId)
            .HasDatabaseName("idx_subscription_plan_benefit_package");

        //Index combinat pentru query-uri
        // Useful pentru: "găsește planuri active de un anumit tip cu un pachet specific"
        builder.HasIndex(sp => new { sp.Type, sp.BenefitPackageId, sp.IsActive })
            .HasDatabaseName("idx_subscription_plan_type_package_active");

        // Relationships
        builder.HasMany(sp => sp.Subscriptions)
            .WithOne(s => s.SubscriptionPlan)
            .HasForeignKey(s => s.SubscriptionPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}