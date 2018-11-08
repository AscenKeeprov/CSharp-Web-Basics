using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panda.Models;
using Panda.Models.Enumerations;

namespace Panda.Data.EntityConfiguration
{
    public class PackageEntityConfiguration : IEntityTypeConfiguration<Package>
    {
	public void Configure(EntityTypeBuilder<Package> entityBuilder)
	{
	    entityBuilder.HasKey(p => p.Id);

	    entityBuilder.Property(p => p.Description)
		.IsUnicode(true);

	    entityBuilder.Property(p => p.Weight)
		.IsRequired(true);

	    entityBuilder.Property(p => p.ShippingAddress)
		.IsRequired(true);

	    entityBuilder.Property(p => p.Status)
		.IsRequired(true)
		.HasDefaultValue(Status.Pending);

	    entityBuilder.Property(p => p.EstimatedDeliveryDate)
		.IsRequired(false)
		.HasDefaultValue(null);

	    entityBuilder.HasOne(p => p.Recipient)
		.WithMany(u => u.Packages)
		.HasForeignKey(p => p.RecipientId);
	}
    }
}
