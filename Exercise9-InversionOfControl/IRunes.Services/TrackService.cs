using System;
using System.Linq;
using IRunes.Data;
using IRunes.Models;
using IRunes.Models.Enumerations;
using IRunes.Services.Contracts;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
	private readonly IRunesDbContext Context;

	public TrackService(IRunesDbContext dbContext)
	{
	    Context = dbContext;
	}

	public void AddTrack(string artist, string title, MusicGenre genre, string link, decimal price)
	{
	    var track = new Track()
	    {
		Artist = artist,
		Title = title,
		Genre = genre,
		Link = link,
		Price = price
	    };
	    Context.Tracks.Add(track);
	    Context.SaveChanges();
	}

	public bool Exists(string trackArtist, string trackTitle)
	{
	    return Context.Tracks.Any(t
		=> t.Artist == trackArtist && t.Title == trackTitle);
	}

	public Track GetTrack(string artist, string title)
	{
	    return Context.Tracks.SingleOrDefault(t
		=> t.Artist == artist && t.Title == title);
	}

	public Track GetTrack(Guid trackId)
	{
	    return Context.Tracks.Find(trackId);
	}
    }
}
