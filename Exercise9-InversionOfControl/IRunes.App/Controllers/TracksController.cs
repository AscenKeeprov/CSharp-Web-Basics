using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using IRunes.App.Common;
using IRunes.App.Controllers.Contracts;
using IRunes.App.ViewModels;
using IRunes.Models.Enumerations;
using IRunes.Services.Contracts;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;
using SIS.Services.Contracts;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller, ITracksController
    {
	private readonly ITrackService TrackService;
	private readonly IAlbumService AlbumService;
	private readonly IAlbumTrackService AlbumTrackService;
	private readonly IEnumerationService Enumerator;

	public TracksController(ITrackService trackService, IAlbumTrackService albumTrackService,
	    IAlbumService albumService, IEnumerationService enumerationService)
	{
	    TrackService = trackService;
	    AlbumService = albumService;
	    AlbumTrackService = albumTrackService;
	    Enumerator = enumerationService;
	}

	[HttpGet]
	public IActionResult Create(TrackCreateGetViewModel model)
	{
	    if (model.IsAuthenticated == "false")
	    {
		model.Error = Constants.UnauthorizedAccessError;
		return Unauthorized(model, Constants.UnauthorizedViewRoute);
	    }
	    Guid albumId = Guid.Parse(model.AlbumId);
	    var album = AlbumService.GetAlbum(albumId);
	    model.AlbumArtist = album.Artist;
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
	    model.MusicGenres = musicGenresList.ToString();
	    return View(model);
	}

	[HttpPost]
	public IActionResult Create(TrackCreatePostViewModel model)
	{
	    Guid albumId = Guid.Parse(model.AlbumId);
	    var album = AlbumService.GetAlbum(albumId);
	    string artist = model.Artist;
	    string title = model.TrackTitle;
	    if (TrackService.Exists(artist, title))
	    {
		model.Error = string.Format(Constants.TrackExistsError, artist, title);
		return View(model);
	    }
	    string genreDisplayName = model.Genre;
	    var genre = Enumerator.ToEnumOrDefault<MusicGenre>(genreDisplayName);
	    string trackUrl = model.Link;
	    decimal price = decimal.Parse(model.Price);
	    TrackService.AddTrack(artist, title, genre, trackUrl, price);
	    var track = TrackService.GetTrack(artist, title);
	    AlbumTrackService.AddAlbumTrack(album.Id, track.Id);
	    model.TrackId = track.Id.ToString();
	    return RedirectTo(string.Format(Constants.TrackDetailsViewRoute, albumId.ToString(), track.Id.ToString()));
	}

	[HttpGet]
	public IActionResult Details(TrackDetailsViewModel model)
	{
	    if (model.IsAuthenticated == "false")
	    {
		model.Error = Constants.UnauthorizedAccessError;
		return Unauthorized(model, Constants.UnauthorizedViewRoute);
	    }
	    Guid trackId = Guid.Parse(model.TrackId);
	    var track = TrackService.GetTrack(trackId);
	    string trackContent = track.Link.Replace("watch?v=", "embed/");
	    model.TrackContent = string.Format(Constants.HtmlIframeTag, 480, 270, trackContent);
	    model.TrackArtist = track.Artist;
	    model.TrackTitle = track.Title;
	    string trackGenre = Enumerator.ToTextOrDefault(typeof(MusicGenre), track.Genre);
	    if (string.IsNullOrEmpty(trackGenre))
	    {
		model.TrackGenre = default(MusicGenre).ToString();
	    }
	    else model.TrackGenre = trackGenre;
	    model.TrackPrice = $"${track.Price:F2}";
	    return View(model);
	}
    }
}
