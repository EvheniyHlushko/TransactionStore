using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionStore.Data.Entities;

namespace TransactionStore.Data.Configurations
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransactionEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentTransactionEntity> builder)
        {
            builder.ToTable("PaymentTransactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Currency).HasMaxLength(3).IsFixedLength();

            builder.Property(t => t.TransactionId).HasMaxLength(50);
            builder.HasIndex(t => t.TransactionId).IsUnique();
        }
    }
}