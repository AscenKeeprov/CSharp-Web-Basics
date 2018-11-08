using IRunes.App.Models;
using IRunes.App.Models.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IRunes.App.Data.EntityConfiguration
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
	public void Configure(EntityTypeBuilder<Album> entityBuilder)
	{
	    entityBuilder.HasKey(a => a.Id);

	    entityBuilder.Property(a => a.Artist)
		.IsRequired(true)
		.IsUnicode(true);

	    entityBuilder.Property(a => a.Title)
		.IsRequired(true)
		.IsUnicode(true);

	    entityBuilder.Property(a => a.Genre)
		.HasDefaultValue(MusicGenre.Unclassified);

	    entityBuilder.HasMany(a => a.AlbumTracks)
		.WithOne(at => at.Album)
		.HasForeignKey(at => at.AlbumId);
	}
    }
}
