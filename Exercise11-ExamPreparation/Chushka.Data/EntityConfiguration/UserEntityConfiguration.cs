using Chushka.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chushka.Data.EntityConfiguration
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

	    entityBuilder.Property(u => u.Email)
		.IsRequired(true)
		.IsUnicode(false);

	    entityBuilder.Property(u => u.Role)
		.IsRequired(true);

	    entityBuilder.HasMany(u => u.Orders)
		.WithOne(o => o.Client)
		.HasForeignKey(o => o.ClientId);
	}
    }
}
