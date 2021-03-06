﻿using SIS.Framework.Models;

namespace IRunes.App.ViewModels
{
    public class AlbumCreatePostViewModel : ViewModel
    {
	public string AlbumTitle { get; set; }
	public string Artist { get; set; }
	public string CoverArt { get; set; }
	public string Genre { get; set; }
	public string MusicGenres { get; set; }
    }
}
