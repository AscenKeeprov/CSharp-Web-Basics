using System;

namespace SIS.Services.Contracts
{
    /// <summary> Facilitates work with enumerated type values during string processing. </summary>
    public interface IEnumerationService
    {
	/// <summary>
	/// Retrieves the display name of a given enumerated type value.
	/// <para>If the value is not valid for that type, null is returned.</para>
	/// </summary>
	string ToTextOrDefault(Type enumType, Enum enumValue);

	/// <summary>
	/// Retrieves the display name of a given enumerated type value.
	/// <para>If the value is not valid for that type, null is returned.</para>
	/// </summary>
	string ToTextOrDefault(Type enumType, int enumValue);

	/// <summary>
	/// Retrieves the display name of a given enumerated type value.
	/// <para>If the value is not valid for that type, null is returned.</para>
	/// </summary>
	string ToTextOrDefault(Type enumType, string enumValue);

	/// <summary>
	/// Retrieves the enumerated type value corresponding to a provided display name.
	/// <para>If no such relation exists, the dafault value for the given type is returned.</para>
	/// </summary>
	TEnum ToEnumOrDefault<TEnum>(string enumName) where TEnum : struct;
    }
}
