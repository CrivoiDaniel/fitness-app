using FitnessApp.Domain.Decorator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations.Decorator;

public class PaymentGatewayLogConfiguration : IEntityTypeConfiguration<PaymentGatewayLog>
{
    public void Configure(EntityTypeBuilder<PaymentGatewayLog> builder)
    {
        builder.ToTable("PaymentGatewayLogs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Provider).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(10).IsRequired();
        builder.Property(x => x.ErrorMessage).HasMaxLength(2000);
        builder.Property(x => x.TransactionId).HasMaxLength(200);

        builder.Property(x => x.Amount).HasPrecision(18, 2);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
    }
}