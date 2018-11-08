namespace IRunes.App.Common
{
    public static class Constants
    {
	public const string AlbumArtistPlaceholder = "@AlbumArtist";
	public const string AlbumCoverArtPlaceholder = "@AlbumCoverArt";
	public const string AlbumCoverArtTooltip = "{0} (Cover Art)";
	public const string AlbumCoverArtTooltipPlaceholder = "@AlbumCoverArtTooltip";
	public const string AlbumDetailsViewRoute = "/Albums/Details";
	public const string AlbumExistsError = "Album '{0} - {1}' already exists!";
	public const string AlbumGenrePlaceholder = "@AlbumGenre";
	public const string AlbumIdPlaceholder = "@AlbumId";
	public const string AlbumNoTracksMessage = "No tracks added to album yet.";
	public const decimal AlbumPriceDiscountMultiplier = 0.87M;
	public const string AlbumPricePlaceholder = "@AlbumPrice";
	public const string AlbumsListEntry = "<a href='/Albums/Details?albumId={0}'>{1}</a>\r\n";
	public const string AlbumsListPlaceholder = "@AlbumsList";
	public const string AlbumsViewRoute = "/Albums/All";
	public const string AlbumTitlePlaceholder = "@AlbumTitle";
	public const string AlbumTracksPlaceholder = "@AlbumTracks";
	public const string CreateAlbumViewRoute = "/Albums/Create";
	public const string CreateTrackViewRoute = "/Tracks/Create";
	public const string DefaultAlbumCoverArt = "/No_Cover_Available.jpg";
	public const string DefaultGuidValueSql = "NEWSEQUENTIALID()";
	public const string ErrorMessagePlaceholder = "@ErrorMessage";
	public const string ErrorMessageTemplate = "/Views/Error.html";
	public const string ErrorTemplatePlaceholder = "@Error";
	public const string HomeViewRoute = "/Home/Index";
	public const string HtmlIframeTag =
	    "<iframe class='embed-responsive-item' width='{0}' height='{1}' src='{2}' allow='encrypted-media' allowfullscreen></iframe>\r\n";
	public const string HtmlImageTag =
	    "<img src='{0}' alt='{1}' height='{2}' width='{3}' style='border: solid 1px' />\r\n";
	public const string HtmlLayout = "/Views/_Layout.html";
	public const string HtmlListItem = "<li class='list-item border-0'>{0}</li>\r\n";
	public const string HtmlListStyleNone = " style='list-style: none;'";
	public const string HtmlViewTemplate = "/Views{0}.html";
	public const string HttpOneProtocolFragment = SIS.HTTP.Common.Constants.HttpOneProtocolFragment;
	public const string InvalidCredentialsError = "Invalid credentials!";
	public const string LoginViewRoute = "/Users/Login";
	public const string LogoutViewRoute = "/Users/Logout";
	public const string MusicGenresListOption = "<option value='{0}'>{0}</option>\r\n";
	public const string MusicGenresPlaceholder = "@MusicGenres";
	public const string NoAlbumsMessage = "No albums available.";
	public const string PageBodyPlaceholder = "@Body";
	public const string PageTitlePlaceholder = "@Title";
	public const string RegisterViewRoute = "/Users/Register";
	public const string ResourceNotAvailableMessage = "Not Available ;(";
	public const string SessionAuthenticationKey = SIS.HTTP.Common.Constants.SessionAuthenticationKey;
	public const string SessionUsernameKey = SIS.HTTP.Common.Constants.SessionUsernameKey;
	public const string TrackArtistPlaceholder = "@TrackArtist";
	public const string TrackContentPlaceholder = "@TrackContent";
	public const string TrackDetailsViewRoute = "/Tracks/Details";
	public const string TrackExistsError = "Track '{0} - {1}' already exists!";
	public const string TrackGenrePlaceholder = "@TrackGenre";
	public const string TrackIdPlaceholder = "@TrackId";
	public const string TrackListEntry = "<a href='/Tracks/Details?albumId={0}&trackId={1}'>{2}</a>\r\n";
	public const string TrackPricePlaceholder = "@TrackPrice";
	public const string TrackTitlePlaceholder = "@TrackTitle";
	public const int UsernameMaxLength = 64;
	public const string UsernameTakenError = "Username '{0}' is already taken!";
    }
}
