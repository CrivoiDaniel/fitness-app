using FitnessApp.Domain.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // Table name
        builder.ToTable("payments");

        // Primary Key
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");

        // Properties
        builder.Property(p => p.SubscriptionId)
            .HasColumnName("subscription_id")
            .IsRequired();

        builder.Property(p => p.Amount)
            .HasColumnName("amount")
            .HasColumnType("DECIMAL(10,2)")
            .IsRequired();

        builder.Property(p => p.PaymentDate)
            .HasColumnName("payment_date")
            .IsRequired();

        builder.Property(p => p.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.InstallmentNumber)
            .HasColumnName("installment_number")
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(p => p.TransactionId)
            .HasColumnName("transaction_id")
            .HasMaxLength(255);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
           .HasColumnName("updated_at")
           .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
           .ValueGeneratedOnAddOrUpdate()
           .IsRequired();

        // Indexes
        builder.HasIndex(p => p.SubscriptionId)
            .HasDatabaseName("idx_payment_subscription");

        builder.HasIndex(p => p.Status)
            .HasDatabaseName("idx_payment_status");

        builder.HasIndex(p => p.PaymentDate)
            .HasDatabaseName("idx_payment_date");

        builder.HasIndex(p => p.TransactionId)
            .IsUnique()
            .HasFilter("transaction_id IS NOT NULL")
            .HasDatabaseName("idx_payment_transaction_unique");

        //(SubscriptionId + InstallmentNumber) UNIQUE
        // Previne duplicate installments pentru același subscription
        builder.HasIndex(p => new { p.SubscriptionId, p.InstallmentNumber })
            .IsUnique()
            .HasDatabaseName("idx_payment_installment_unique");

        // Relationship is defined in Subscription configuration
    }
}