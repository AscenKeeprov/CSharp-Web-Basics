using SIS.Framework.Models;

namespace IRunes.App.ViewModels
{
    public class TrackDetailsViewModel : ViewModel
    {
	public string AlbumId { get; set; }
	public string TrackArtist { get; set; }
	public string TrackContent { get; set; }
	public string TrackGenre { get; set; }
	public string TrackId { get; set; }
	public string TrackPrice { get; set; }
	public string TrackTitle { get; set; }
    }
}
