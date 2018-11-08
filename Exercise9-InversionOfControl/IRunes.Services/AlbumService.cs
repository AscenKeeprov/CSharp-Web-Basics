using System;
using System.Collections.Generic;
using System.Linq;
using IRunes.Data;
using IRunes.Models;
using IRunes.Models.Enumerations;
using IRunes.Services.Contracts;

namespace IRunes.Services
{
    public class AlbumService : IAlbumService
    {
	private readonly IRunesDbContext Context;

	public AlbumService(IRunesDbContext dbContext)
	{
	    Context = dbContext;
	}

	public void AddAlbum(string artist, string title, MusicGenre genre, string coverArt)
	{
	    Album album = new Album()
	    {
		Artist = artist,
		Title = title,
		Genre = genre,
		CoverArt = coverArt
	    };
	    Context.Albums.Add(album);
	    Context.SaveChanges();
	}

	public bool Exists(string albumArtist, string albumTitle)
	{
	    return Context.Albums.Any(a
		=> a.Artist == albumArtist && a.Title == albumTitle);
	}

	public Album GetAlbum(Guid albumId)
	{
	    return Context.Albums.Find(albumId);
	}

	public Album GetAlbum(string artist, string title)
	{
	    return Context.Albums.SingleOrDefault(a
		=> a.Artist == artist && a.Title == title);
	}

	public IEnumerable<Album> GetAlbums()
	{
	    var albums = Context.Albums
		.OrderBy(a => a.Artist)
		.ThenBy(a => a.Title)
		.AsEnumerable();
	    return albums;
	}

	public IEnumerable<Track> GetAlbumTracks(Album album)
	{
	    var albumTracks = album.AlbumTracks
		.Select(at => at.Track);
	    return albumTracks;
	}
    }
}
