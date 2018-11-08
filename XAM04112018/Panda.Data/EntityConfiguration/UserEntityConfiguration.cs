using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panda.Models;

namespace Panda.Data.EntityConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
	public void Configure(EntityTypeBuilder<User> entityBuilder)
	{
	    entityBuilder.HasKey(u => u.Id);

	    entityBuilder.Property(u => u.Username)
		.IsRequired(true)
		.IsUnicode(false)
		.HasMaxLength(64);

	    entityBuilder.Property(u => u.Password)
		.IsRequired(true)
		.IsUnicode(false);

	    entityBuilder.Property(u => u.Role)
		.IsRequired(true);

	    entityBuilder.HasMany(u => u.Packages)
		.WithOne(p => p.Recipient)
		.HasForeignKey(p => p.RecipientId);

	    entityBuilder.HasMany(u => u.Receipts)
		.WithOne(r => r.Recipient)
		.HasForeignKey(r => r.RecipientId);
	}
    }
}
