namespace IRunes.App.Common
{
    public static class Constants
    {
	public const string AlbumArtistPlaceholder = "@AlbumArtist";
	public const string AlbumCoverArtPlaceholder = "@AlbumCoverArt";
	public const string AlbumCoverArtTooltip = "{0} - {1} (Cover Art)";
	public const string AlbumCoverArtValue =
	    "<img src='{0}' alt='{1}' height='256' width='256' style='border: solid 1px' />";
	public const string AlbumDetailsViewRoute = "/Albums/Details";
	public const string AlbumGenrePlaceholder = "@AlbumGenre";
	public const string AlbumIdPlaceholder = "@AlbumId";
	public const string AlbumNoTracksMessage = "No tracks added to album yet.";
	public const decimal AlbumPriceDiscountMultiplier = 0.87M;
	public const string AlbumPricePlaceholder = "@AlbumPrice";
	public const string AlbumsCoverArtDir = "../../../Resources/Albums/CoverArt/";
	public const string AlbumsListEntry = "<p><a href='/Albums/Details?albumId={0}'>{1}</a></p>";
	public const string AlbumsListPlaceholder = "@AlbumsList";
	public const string AlbumsViewRoute = "/Albums/All";
	public const string AlbumTitlePlaceholder = "@AlbumTitle";
	public const string AlbumTracksPlaceholder = "@AlbumTracks";
	public const string CreateAlbumViewRoute = "/Albums/Create";
	public const string CreateTrackViewRoute = "/Tracks/Create";
	public const string DefaultAlbumCoverArt =
	    "https://upload.wikimedia.org/wikipedia/commons/b/b9/No_Cover.jpg";
	public const string DefaultGuidValueSql = "NEWSEQUENTIALID()";
	public const string DefaultPageTitle = "IRunes";
	public const string EmailPlaceholder = "@Email";
	public const string ErrorMessage = @"<h3 style='color: orangered'>{0}</h3>";
	public const string FavIconRoute = "../../../Views/favicon.ico";
	public const string FirstNamePlaceholder = "@FirstName";
	public const string HomeViewRoute = "/Home/Index";
	public const string HtmlBodyPath = "../../../Views{0}.html";
	public const string HtmlLayoutPath = "../../../Views/_Layout.html";
	public const string InvalidCredentialsError = "Invalid credentials!";
	public const string LastNamePlaceholder = "@LastName";
	public const string LoginViewRoute = "/Users/Login";
	public const string LogoutViewRoute = "/Users/Logout";
	public const string ModelDataPlaceholder = "@Model.";
	public const string MusicGenreListOption = "<option value='{0}'>{0}</option>\r\n";
	public const string MusicGenresPlaceholder = "@MusicGenres";
	public const string NoAlbumsMessage = "No albums available.";
	public const string NotFoundViewRoute = "/NotFound";
	public const string PageBodyPlaceholder = "@Body";
	public const string PageErrorPlaceholder = "@Error";
	public const string PageTitlePlaceholder = "@Title";
	public const string PasswordSalt = "*ƤåšśŵöȑđŜäɬť*";
	public const string RegisterViewRoute = "/Users/Register";
	public const string ResourceNotAvailableMessage = "Not Available ;(";
	public const string SessionAuthenticationKey = SIS.HTTP.Common.Constants.SessionAuthenticationKey;
	public const string SessionDefaultUsername = SIS.HTTP.Common.Constants.DefaultSessionUsername;
	public const string SessionUsernameKey = SIS.HTTP.Common.Constants.SessionUsernameKey;
	public const string TrackArtistPlaceholder = "@TrackArtist";
	public const string TrackContentPlaceholder = "@TrackContent";
	public const string TrackDetailsViewRoute = "/Tracks/Details";
	public const string TrackGenrePlaceholder = "@TrackGenre";
	public const string TrackIdPlaceholder = "@TrackId";
	public const string TrackIframePlaceholder =
	    "<iframe width='384' height='240' src='{0}' frameborder='0' allow='encrypted-media' allowfullscreen></iframe>";
	public const string TrackListEntry =
	    "<li><a href='/Tracks/Details?albumId={0}&trackId={1}'>{2}</a></li>";
	public const string TrackPricePlaceholder = "@TrackPrice";
	public const string TrackTitlePlaceholder = "@TrackTitle";
	public const int UsernameMaxLength = 64;
	public const string UsernameTakenError = "Username '{0}' is already taken!";
    }
}
