using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using IRunes.App.Common;
using IRunes.App.Controllers.Contracts;
using IRunes.App.Models;
using IRunes.App.Models.Enumerations;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.App.Controllers
{
    public class TracksController : BaseController, ITracksController
    {
	public TracksController(IServiceProvider services) : base(services) { }

	public IHttpResponse BrowseOne(IHttpRequest request)
	{
	    Guid trackId = Guid.Parse(request.QueryData["trackId"].ToString()
		.Split('=', StringSplitOptions.RemoveEmptyEntries)[1]);
	    var track = Context.Tracks.Find(trackId);
	    string trackContent = track.Link.Replace("watch?v=", "embed/");
	    viewBag[Constants.TrackContentPlaceholder] = string
		.Format(Constants.TrackIframePlaceholder, trackContent);
	    viewBag[Constants.TrackArtistPlaceholder] = track.Artist;
	    viewBag[Constants.TrackTitlePlaceholder] = track.Title;
	    var trackGenre = typeof(MusicGenre)
		.GetMember(track.Genre.ToString())[0]
		.GetCustomAttribute<DescriptionAttribute>();
	    if (trackGenre == null) viewBag[Constants.TrackGenrePlaceholder] = track.Genre.ToString();
	    else viewBag[Constants.TrackGenrePlaceholder] = trackGenre.Description;
	    viewBag[Constants.TrackPricePlaceholder] = $"${track.Price:F2}";
	    viewBag[Constants.AlbumIdPlaceholder] = request.QueryData["albumId"].ToString()
		.Split('=', StringSplitOptions.RemoveEmptyEntries)[1];
	    return BuildView(request.Path, request);
	}

	public IHttpResponse CreateGet(IHttpRequest request)
	{
	    Guid albumId = Guid.Parse(request.QueryData["albumId"].ToString()
		.Split('=', StringSplitOptions.RemoveEmptyEntries)[1]);
	    viewBag[Constants.AlbumIdPlaceholder] = albumId.ToString();
	    var album = Context.Albums.Find(albumId);
	    viewBag[Constants.AlbumArtistPlaceholder] = album.Artist;
	    var musicGenres = Enum.GetValues(typeof(MusicGenre));
	    StringBuilder genresList = new StringBuilder();
	    foreach (var musicGenre in musicGenres)
	    {
		var genre = musicGenre.GetType().GetMember(musicGenre.ToString())
		    .First().GetCustomAttribute<DescriptionAttribute>();
		if (genre != null)
		{
		    genresList.Append(string.Format(Constants.MusicGenreListOption, genre.Description));
		}
		else genresList.Append(string.Format(Constants.MusicGenreListOption, musicGenre.ToString()));
	    }
	    viewBag[Constants.MusicGenresPlaceholder] = genresList.ToString();
	    return BuildView(request.Path, request);
	}

	public IHttpResponse CreatePost(IHttpRequest request)
	{
	    Guid albumId = Guid.Parse(request.QueryData["albumId"].ToString()
		.Split('=', StringSplitOptions.RemoveEmptyEntries)[1]);
	    viewBag[Constants.AlbumIdPlaceholder] = albumId.ToString();
	    var album = Context.Albums.Find(albumId);
	    string trackArtist = request.FormData["artist"].ToString().Split('=')[1];
	    string trackTitle = request.FormData["title"].ToString().Split('=')[1];
	    if (Context.Tracks.Any(t => t.Artist == trackArtist && t.Title == trackTitle))
	    {
		viewBag[Constants.PageErrorPlaceholder] = string.Format(
		    Constants.ErrorMessage, $"Track '{trackArtist} - {trackTitle}' already exists!");
		return BuildView(request.Path, request);
	    }
	    var genreDescription = request.FormData["genre"].ToString().Split('=')[1];
	    if (!Enum.TryParse(typeof(MusicGenre), genreDescription, true, out object genre))
	    {
		genre = typeof(MusicGenre).GetFields()
		.SelectMany(f => f.GetCustomAttributes(typeof(DescriptionAttribute), false), (f, a) => new { Field = f, Att = a })
		.Where(a => ((DescriptionAttribute)a.Att).Description == genreDescription)
		.SingleOrDefault().Field.Name;
	    }
	    string trackUrl = request.FormData["link"].ToString().Split('=', 2)[1];
	    decimal trackPrice = decimal.Parse(request.FormData["price"].ToString().Split('=')[1]);
	    var track = new Track()
	    {
		Artist = trackArtist,
		Title = trackTitle,
		Genre = Enum.Parse<MusicGenre>(genre.ToString()),
		Link = trackUrl,
		Price = trackPrice
	    };
	    Context.Tracks.Add(track);
	    var albumTrack = new AlbumTrack()
	    {
		AlbumId = albumId,
		TrackId = track.Id
	    };
	    album.AlbumTracks.Add(albumTrack);
	    album.Price += track.Price;
	    Context.SaveChanges();
	    request.QueryData["trackId"] = $"trackId={track.Id.ToString()}";
	    BrowseOne(request);
	    return BuildView(Constants.TrackDetailsViewRoute, request, HttpResponseStatusCode.SeeOther);
	}
    }
}
