using System.ComponentModel;

namespace IRunes.App.Models.Enumerations
{
    public enum MusicGenre
    {
	Unclassified,
	[Description("Alternative Metal")]
	AlternativeMetal,
	[Description("Alternative Rock")]
	AlternativeRock,
	Blues,
	[Description("Blues Rock")]
	BluesRock,
	Classical,
	Country,
	Dance,
	Disco,
	[Description("Drum & Bass")]
	DrumNBass,
	Dubstep,
	Electro,
	[Description("Electronic Rock")]
	ElectronicRock,
	Europop,
	Folk,
	Funk,
	Grunge,
	Hardcore,
	[Description("Heavy Metal")]
	HeavyMetal,
	[Description("Hip-Hop")]
	HipHop,
	House,
	Industrial,
	[Description("Industrial Metal")]
	IndustrialMetal,
	Jazz,
	[Description("K-Pop")]
	KPop,
	Latin,
	Metal,
	Metalcore,
	Newage,
	Pop,
	[Description("Pop Rock")]
	PopRock,
	[Description("Progressive Metal")]
	ProgressiveMetal,
	[Description("Psychedelic Rock")]
	PsychedelicRock,
	[Description("Punk Rock")]
	PunkRock,
	Reggae,
	Religious,
	Rock,
	Soul,
	Techno,
	Trance,
	Tribal
    }
}
