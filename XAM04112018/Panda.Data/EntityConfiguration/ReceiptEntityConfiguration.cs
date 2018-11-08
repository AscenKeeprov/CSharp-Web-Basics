using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panda.Models;

namespace Panda.Data.EntityConfiguration
{
    public class ReceiptEntityConfiguration : IEntityTypeConfiguration<Receipt>
    {
	public void Configure(EntityTypeBuilder<Receipt> entityBuilder)
	{
	    entityBuilder.HasKey(r => r.Id);

	    entityBuilder.Property(r => r.Id)
		.UseSqlServerIdentityColumn();

	    entityBuilder.Property(r => r.Fee)
		.IsRequired(true)
		.HasDefaultValue(0.01M);

	    entityBuilder.Property(r => r.IssuedOn)
		.IsRequired(true)
		.HasDefaultValueSql("GETUTCDATE()");

	    entityBuilder.HasOne(r => r.Package)
		.WithOne(p => p.Receipt)
		.HasForeignKey<Receipt>(r => r.PackageId)
		.OnDelete(DeleteBehavior.Restrict);

	    entityBuilder.HasOne(r => r.Recipient)
		.WithMany(r => r.Receipts)
		.HasForeignKey(r => r.RecipientId)
		.OnDelete(DeleteBehavior.Restrict);
	}
    }
}
