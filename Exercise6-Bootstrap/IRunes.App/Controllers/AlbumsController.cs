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
    public class AlbumsController : BaseController, IAlbumsController
    {
	private IEnumerationService Enumerator => (EnumerationService)services
	    .GetService(typeof(IEnumerationService));

	public AlbumsController(IServiceProvider services) : base(services) { }

	public IHttpResponse BrowseAll(IHttpRequest request)
	{
	    var albums = Context.Albums
		.OrderBy(a => a.Artist)
		.ThenBy(a => a.Title).ToList();
	    if (albums.Count == 0)
	    {
		viewBag[Constants.AlbumsListPlaceholder] = Constants.NoAlbumsMessage;
		return BuildView(request.Path, request);
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
	    viewBag[Constants.AlbumsListPlaceholder] = albumsList.ToString();
	    return BuildView(request.Path, request);
	}

	public IHttpResponse BrowseOne(IHttpRequest request)
	{
	    Guid albumId = Guid.Parse(request.QueryData["albumId"].ToString()
		.Split('=', StringSplitOptions.RemoveEmptyEntries)[1]);
	    var album = Context.Albums.Find(albumId);
	    viewBag[Constants.AlbumIdPlaceholder] = album.Id.ToString();
	    if (string.IsNullOrEmpty(album.CoverArt))
	    {
		viewBag[Constants.AlbumCoverArtPlaceholder] = Constants.DefaultAlbumCoverArt;
		viewBag[Constants.AlbumCoverArtTooltipPlaceholder] = Constants.ResourceNotAvailableMessage;
	    }
	    else
	    {
		viewBag[Constants.AlbumCoverArtPlaceholder] = album.CoverArt;
		viewBag[Constants.AlbumCoverArtTooltipPlaceholder] = string
		    .Format(Constants.AlbumCoverArtTooltip, album.ToString());
	    }
	    viewBag[Constants.AlbumArtistPlaceholder] = album.Artist;
	    viewBag[Constants.AlbumTitlePlaceholder] = album.Title;
	    string albumGenre = Enumerator.ToTextOrDefault(typeof(MusicGenre), album.Genre);
	    if (string.IsNullOrEmpty(albumGenre))
	    {
		viewBag[Constants.AlbumGenrePlaceholder] = default(MusicGenre).ToString();
	    }
	    else viewBag[Constants.AlbumGenrePlaceholder] = albumGenre;
	    if (!album.AlbumTracks.Any())
	    {
		viewBag[Constants.AlbumPricePlaceholder] = "&mdash;";
		viewBag[Constants.AlbumTracksPlaceholder] = Constants.AlbumNoTracksMessage;
	    }
	    else
	    {
		Track[] albumTracks = album.AlbumTracks.Select(at => at.Track).ToArray();
		decimal albumPrice = albumTracks.Sum(track => track.Price);
		if (albumTracks.Length > 1)
		{
		    albumPrice *= Constants.AlbumPriceDiscountMultiplier;
		    StringBuilder albumTrackList = new StringBuilder();
		    for (int t = 1; t <= albumTracks.Length; t++)
		    {
			Track track = albumTracks[t - 1];
			string trackListEntry = string.Format(Constants.TrackListEntry,
			    album.Id.ToString(), track.Id.ToString(), track.Title);
			string trackListItem = string.Format(Constants.HtmlListItem,
			    $"<b>{t}</b>.&nbsp;<i>{trackListEntry}</i>\r\n");
			albumTrackList.Append(trackListItem);
		    }
		    viewBag[Constants.AlbumTracksPlaceholder] = albumTrackList.ToString();
		}
		else
		{
		    StringBuilder singleTrackInfo = new StringBuilder();
		    string trackLine = $"<i>{albumTracks[0].Title}</i>\r\n";
		    string trackListEntry = string.Format(Constants.TrackListEntry,
			    album.Id.ToString(), albumTracks[0].Id.ToString(), trackLine);
		    string trackListItem = string.Format(Constants.HtmlListItem, trackListEntry);
		    singleTrackInfo.Append(trackListItem);
		    viewBag[Constants.AlbumTracksPlaceholder] = singleTrackInfo.ToString();
		}
		viewBag[Constants.AlbumPricePlaceholder] = $"${albumPrice:F2}";
	    }
	    return BuildView(request.Path, request);
	}

	public IHttpResponse CreateGet(IHttpRequest request)
	{
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
	    string artist = request.FormData["artist"].ToString().Split('=')[1];
	    string title = request.FormData["title"].ToString().Split('=')[1];
	    if (Context.Albums.Any(a => a.Artist == artist && a.Title == title))
	    {
		viewBag[Constants.ErrorMessagePlaceholder] = string
		    .Format(Constants.AlbumExistsError, artist, title);
		return BuildView(request.Path, request);
	    }
	    string genreDisplayName = request.FormData["genre"].ToString().Split('=')[1];
	    var genre = Enumerator.ToEnumOrDefault<MusicGenre>(genreDisplayName);
	    string coverArt = request.FormData["coverArt"].ToString().Split('=')[1];
	    var album = new Album()
	    {
		Artist = artist,
		Title = title,
		Genre = genre,
		CoverArt = coverArt
	    };
	    Context.Albums.Add(album);
	    Context.SaveChanges();
	    return BuildView(Constants.AlbumsViewRoute, request, HttpResponseStatusCode.SeeOther);
	}
    }
}
