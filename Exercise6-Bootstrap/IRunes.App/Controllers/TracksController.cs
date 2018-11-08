using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using IRunes.App.Common;
using IRunes.App.Controllers.Contracts;
using IRunes.App.Models;
using IRunes.App.Models.Enumerations;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.Services;
using SIS.Services.Contracts;

namespace IRunes.App.Controllers
{
    public class TracksController : BaseController, ITracksController
    {
	private IEnumerationService Enumerator => (EnumerationService)services
	    .GetService(typeof(IEnumerationService));

	public TracksController(IServiceProvider services) : base(services) { }

	public IHttpResponse BrowseOne(IHttpRequest request)
	{
	    Guid trackId = Guid.NewGuid();
	    if (request.QueryData.ContainsKey("trackId"))
	    {
		trackId = Guid.Parse(request.QueryData["trackId"].ToString()
		    .Split('=', StringSplitOptions.RemoveEmptyEntries)[1]);
		viewBag[Constants.TrackIdPlaceholder] = trackId.ToString();
	    }
	    else if (viewBag.ContainsKey(Constants.TrackIdPlaceholder))
	    {
		trackId = Guid.Parse(viewBag[Constants.TrackIdPlaceholder]);
	    }
	    else return BuildView(request.Path, request, HttpResponseStatusCode.BadRequest);
	    string albumId = string.Empty;
	    if (request.QueryData.ContainsKey("albumId"))
	    {
		albumId = request.QueryData["albumId"].ToString()
		    .Split('=', StringSplitOptions.RemoveEmptyEntries)[1];
		viewBag[Constants.AlbumIdPlaceholder] = albumId;
	    }
	    else if (viewBag.ContainsKey(Constants.AlbumIdPlaceholder))
	    {
		albumId = viewBag[Constants.AlbumIdPlaceholder];
	    }
	    else return BuildView(request.Path, request, HttpResponseStatusCode.BadRequest);
	    var track = Context.Tracks.Find(trackId);
	    string trackContent = track.Link.Replace("watch?v=", "embed/");
	    viewBag[Constants.TrackContentPlaceholder] = string
		.Format(Constants.HtmlIframeTag, 480, 270, trackContent);
	    viewBag[Constants.TrackArtistPlaceholder] = track.Artist;
	    viewBag[Constants.TrackTitlePlaceholder] = track.Title;
	    string trackGenre = Enumerator.ToTextOrDefault(typeof(MusicGenre), track.Genre);
	    if (string.IsNullOrEmpty(trackGenre))
	    {
		viewBag[Constants.TrackGenrePlaceholder] = default(MusicGenre).ToString();
	    }
	    else viewBag[Constants.TrackGenrePlaceholder] = trackGenre;
	    viewBag[Constants.TrackPricePlaceholder] = $"${track.Price:F2}";
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
	    StringBuilder musicGenresList = new StringBuilder();
	    foreach (var musicGenre in musicGenres)
	    {
		var genre = musicGenre.GetType().GetMember(musicGenre.ToString())
		    .First().GetCustomAttribute<DisplayNameAttribute>();
		if (genre != null)
		{
		    musicGenresList.AppendFormat(Constants.MusicGenresListOption, genre.DisplayName);
		}
		else musicGenresList.AppendFormat(Constants.MusicGenresListOption, musicGenre.ToString());
	    }
	    viewBag[Constants.MusicGenresPlaceholder] = musicGenresList.ToString();
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
		viewBag[Constants.ErrorMessagePlaceholder] = string
		    .Format(Constants.TrackExistsError, trackArtist, trackTitle);
		return BuildView(request.Path, request);
	    }
	    string genreDisplayName = request.FormData["genre"].ToString().Split('=')[1];
	    var genre = Enumerator.ToEnumOrDefault<MusicGenre>(genreDisplayName);
	    string trackUrl = request.FormData["link"].ToString().Split('=', 2)[1];
	    decimal trackPrice = decimal.Parse(request.FormData["price"].ToString().Split('=')[1]);
	    var track = new Track()
	    {
		Artist = trackArtist,
		Title = trackTitle,
		Genre = genre,
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
	    viewBag[Constants.TrackIdPlaceholder] = track.Id.ToString();
	    return BuildView(Constants.TrackDetailsViewRoute, request, HttpResponseStatusCode.SeeOther);
	}
    }
}
