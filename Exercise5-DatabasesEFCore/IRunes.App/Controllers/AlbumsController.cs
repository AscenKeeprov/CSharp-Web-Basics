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
    public class AlbumsController : BaseController, IAlbumsController
    {
	public AlbumsController(IServiceProvider services) : base(services) { }

	public IHttpResponse BrowseAll(IHttpRequest request)
	{
	    var albums = Context.Albums.ToList();
	    if (albums.Count == 0)
	    {
		viewBag[Constants.AlbumsListPlaceholder] = Constants.NoAlbumsMessage;
		return BuildView(request.Path, request);
	    }
	    StringBuilder albumsList = new StringBuilder();
	    foreach (var album in albums)
	    {
		albumsList.Append(string.Format(Constants.AlbumsListEntry,
		    album.Id, $"{album.Artist} - {album.Title}"));
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
		viewBag[Constants.AlbumCoverArtPlaceholder] = string.Format(Constants.AlbumCoverArtValue,
		    Constants.DefaultAlbumCoverArt, Constants.ResourceNotAvailableMessage);
	    }
	    else
	    {
		viewBag[Constants.AlbumCoverArtPlaceholder] = string.Format(Constants.AlbumCoverArtValue,
		    album.CoverArt, string.Format(Constants.AlbumCoverArtTooltip, album.Artist, album.Title));
	    }
	    viewBag[Constants.AlbumArtistPlaceholder] = album.Artist;
	    viewBag[Constants.AlbumTitlePlaceholder] = album.Title;
	    var albumGenre = typeof(MusicGenre)
		.GetMember(album.Genre.ToString())[0]
		.GetCustomAttribute<DescriptionAttribute>();
	    if (albumGenre == null) viewBag[Constants.AlbumGenrePlaceholder] = album.Genre.ToString();
	    else viewBag[Constants.AlbumGenrePlaceholder] = albumGenre.Description;
	    if (!album.AlbumTracks.Any())
	    {
		viewBag[Constants.AlbumPricePlaceholder] = "&mdash;";
		viewBag[Constants.AlbumTracksPlaceholder] = Constants.AlbumNoTracksMessage;
	    }
	    else
	    {
		Track[] albumTracks = album.AlbumTracks.Select(at => at.Track).ToArray();
		decimal albumPrice = albumTracks.Sum(track => track.Price);
		StringBuilder albumTrackList = new StringBuilder("<ul>");
		if (albumTracks.Length > 1)
		{
		    albumPrice *= Constants.AlbumPriceDiscountMultiplier;
		    for (int t = 0; t < albumTracks.Length; t++)
		    {
			albumTrackList.Append(string.Format(Constants.TrackListEntry, album.Id.ToString(),
			    albumTracks[t].Id, $"{t + 1}.&nbsp;{albumTracks[t].Title}"));
		    }
		}
		else albumTrackList.Append(string.Format(Constants.TrackListEntry,
			    album.Id.ToString(), albumTracks[0].Id, albumTracks[0].Title));
		albumTrackList.Append("</ul>");
		viewBag[Constants.AlbumPricePlaceholder] = $"${albumPrice:F2}";
		viewBag[Constants.AlbumTracksPlaceholder] = albumTrackList.ToString();
	    }
	    return BuildView(request.Path, request);
	}

	public IHttpResponse CreateGet(IHttpRequest request)
	{
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
	    string artist = request.FormData["artist"].ToString().Split('=')[1];
	    string title = request.FormData["title"].ToString().Split('=')[1];
	    if (Context.Albums.Any(a => a.Artist == artist && a.Title == title))
	    {
		viewBag[Constants.PageErrorPlaceholder] = string.Format(
		    Constants.ErrorMessage, $"Album '{artist} - {title}' already exists!");
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
	    string coverArt = request.FormData["coverArt"].ToString().Split('=')[1];
	    var album = new Album()
	    {
		Artist = artist,
		Title = title,
		Genre = Enum.Parse<MusicGenre>(genre.ToString()),
		CoverArt = coverArt
	    };
	    Context.Albums.Add(album);
	    Context.SaveChanges();
	    return BuildView(Constants.AlbumsViewRoute, request, HttpResponseStatusCode.SeeOther);
	}
    }
}
