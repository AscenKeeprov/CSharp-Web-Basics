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
    public class AlbumsController : Controller, IAlbumsController
    {
	private readonly IAlbumService AlbumService;
	private readonly IEnumerationService Enumerator;

	public AlbumsController(IAlbumService albumService, IEnumerationService enumerationService)
	{
	    AlbumService = albumService;
	    Enumerator = enumerationService;
	}

	[HttpGet]
	public IActionResult All(AlbumsViewModel model)
	{
	    if (model.IsAuthenticated == "false")
	    {
		model.Error = Constants.UnauthorizedAccessError;
		return Unauthorized(model, Constants.UnauthorizedViewRoute);
	    }
	    var albums = AlbumService.GetAlbums();
	    if (albums.Count() == 0)
	    {
		model.AlbumsList = Constants.NoAlbumsMessage;
		return View(model);
	    }
	    StringBuilder albumsList = new StringBuilder();
	    foreach (var album in albums)
	    {
		string albumsListEntry = string.Format(
		    Constants.AlbumsListEntry, album.Id, album.ToString());
		string albumsListItem = string.Format(
		    Constants.HtmlListItem, $"<b>{albumsListEntry}</b>\r\n");
		albumsList.Append(albumsListItem);
	    }
	    model.AlbumsList = albumsList.ToString();
	    return View(model);
	}

	[HttpGet]
	public IActionResult Create(AlbumCreateGetViewModel model)
	{
	    if (model.IsAuthenticated == "false")
	    {
		model.Error = Constants.UnauthorizedAccessError;
		return Unauthorized(model, Constants.UnauthorizedViewRoute);
	    }
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
	public IActionResult Create(AlbumCreatePostViewModel model)
	{
	    string artist = model.Artist;
	    string title = model.AlbumTitle;
	    if (AlbumService.Exists(artist, title))
	    {
		model.Error = string.Format(Constants.AlbumExistsError, artist, title);
		return View(model);
	    }
	    string genreDisplayName = model.Genre;
	    var genre = Enumerator.ToEnumOrDefault<MusicGenre>(genreDisplayName);
	    string coverArt = model.CoverArt;
	    AlbumService.AddAlbum(artist, title, genre, coverArt);
	    return RedirectTo(Constants.AlbumsViewRoute);
	}

	[HttpGet]
	public IActionResult Details(AlbumDetailsViewModel model)
	{
	    if (model.IsAuthenticated == "false")
	    {
		model.Error = Constants.UnauthorizedAccessError;
		return Unauthorized(model, Constants.UnauthorizedViewRoute);
	    }
	    Guid albumId = Guid.Parse(model.AlbumId);
	    var album = AlbumService.GetAlbum(albumId);
	    if (string.IsNullOrEmpty(album.CoverArt))
	    {
		model.AlbumCoverArt = Constants.DefaultAlbumCoverArt;
		model.AlbumCoverArtTooltip = Constants.ResourceNotAvailableMessage;
	    }
	    else
	    {
		model.AlbumCoverArt = album.CoverArt;
		model.AlbumCoverArtTooltip = string.Format(Constants.AlbumCoverArtTooltip, album.ToString());
	    }
	    model.AlbumArtist = album.Artist;
	    model.AlbumTitle = album.Title;
	    string albumGenre = Enumerator.ToTextOrDefault(typeof(MusicGenre), album.Genre);
	    if (string.IsNullOrEmpty(albumGenre))
	    {
		model.AlbumGenre = default(MusicGenre).ToString();
	    }
	    else model.AlbumGenre = albumGenre;
	    if (!album.AlbumTracks.Any())
	    {
		model.AlbumPrice = "&mdash;";
		model.AlbumTracks = Constants.AlbumNoTracksMessage;
	    }
	    else
	    {
		var albumTracks = AlbumService.GetAlbumTracks(album).ToArray();
		decimal albumPrice = albumTracks.Sum(track => track.Price);
		if (albumTracks.Length > 1)
		{
		    albumPrice *= Constants.AlbumPriceDiscountMultiplier;
		    StringBuilder albumTrackList = new StringBuilder();
		    for (int t = 1; t <= albumTracks.Length; t++)
		    {
			var track = albumTracks[t - 1];
			string trackListEntry = string.Format(Constants.TrackListEntry,
			    album.Id.ToString(), track.Id.ToString(), track.Title);
			string trackListItem = string.Format(Constants.HtmlListItem,
			    $"<b>{t}</b>.&nbsp;<i>{trackListEntry}</i>\r\n");
			albumTrackList.Append(trackListItem);
		    }
		    model.AlbumTracks = albumTrackList.ToString();
		}
		else
		{
		    StringBuilder singleTrackInfo = new StringBuilder();
		    string trackLine = $"<i>{albumTracks[0].Title}</i>\r\n";
		    string trackListEntry = string.Format(Constants.TrackListEntry,
			    album.Id.ToString(), albumTracks[0].Id.ToString(), trackLine);
		    string trackListItem = string.Format(Constants.HtmlListItem, trackListEntry);
		    singleTrackInfo.Append(trackListItem);
		    model.AlbumTracks = singleTrackInfo.ToString();
		}
		model.AlbumPrice = $"${albumPrice:F2}";
	    }
	    return View(model);
	}
    }
}
