using System.ComponentModel;

namespace IRunes.Models.Enumerations
{
    public enum MusicGenre
    {
	Unclassified = 0,
	[DisplayName("Alternative Metal")]
	AlternativeMetal,
	[DisplayName("Alternative Rock")]
	AlternativeRock,
	Blues,
	[DisplayName("Blues Rock")]
	BluesRock,
	Classical,
	Country,
	Dance,
	Disco,
	[DisplayName("Drum & Bass")]
	DrumNBass,
	Dubstep,
	Electro,
	[DisplayName("Electronic Rock")]
	ElectronicRock,
	Europop,
	Folk,
	Funk,
	Grunge,
	Hardcore,
	[DisplayName("Heavy Metal")]
	HeavyMetal,
	[DisplayName("Hip-Hop")]
	HipHop,
	House,
	Industrial,
	[DisplayName("Industrial Metal")]
	IndustrialMetal,
	Jazz,
	[DisplayName("K-Pop")]
	KPop,
	Latin,
	Metal,
	Metalcore,
	Newage,
	Pop,
	[DisplayName("Pop Rock")]
	PopRock,
	[DisplayName("Progressive Metal")]
	ProgressiveMetal,
	[DisplayName("Psychedelic Rock")]
	PsychedelicRock,
	[DisplayName("Punk Rock")]
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
