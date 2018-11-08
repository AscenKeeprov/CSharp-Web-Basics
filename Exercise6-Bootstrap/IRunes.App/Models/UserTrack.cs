using System;

namespace IRunes.App.Models
{
    public class UserTrack
    {
	public Guid UserId { get; set; }
	public virtual User User { get; set; }

	public Guid TrackId { get; set; }
	public virtual Track Track { get; set; }
    }
}
