using FitnessApp.Domain.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        // Table name
        builder.ToTable("subscriptions");

        // Primary Key
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");

        // Properties
        builder.Property(s => s.ClientId)
            .HasColumnName("client_id")
            .IsRequired();

        builder.Property(s => s.SubscriptionPlanId)
            .HasColumnName("subscription_plan_id")
            .IsRequired();

        builder.Property(s => s.StartDate)
            .HasColumnName("start_date")
            .HasColumnType("DATE")
            .IsRequired();

        builder.Property(s => s.EndDate)
            .HasColumnName("end_date")
            .HasColumnType("DATE");

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(s => s.AutoRenew)
            .HasColumnName("auto_renew")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Indexes
        builder.HasIndex(s => s.ClientId)
            .HasDatabaseName("idx_subscription_client");

        builder.HasIndex(s => s.SubscriptionPlanId)
            .HasDatabaseName("idx_subscription_plan");

        builder.HasIndex(s => s.Status)
            .HasDatabaseName("idx_subscription_status");

        builder.HasIndex(s => s.EndDate)
            .HasDatabaseName("idx_subscription_end_date");

        //Index combinat pentru query-uri frecvente
        // Useful pentru: "găsește toate subscriptions active ale unui client pentru un plan"
        builder.HasIndex(s => new { s.ClientId, s.SubscriptionPlanId, s.Status })
            .HasDatabaseName("idx_subscription_client_plan_status");

        // Relationships
        builder.HasOne(s => s.Client)
            .WithMany(c => c.Subscriptions)
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Payments)
            .WithOne(p => p.Subscription)
            .HasForeignKey(p => p.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}