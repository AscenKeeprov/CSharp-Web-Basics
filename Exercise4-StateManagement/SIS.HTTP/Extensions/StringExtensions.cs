using System.Globalization;
using System.Threading;

namespace SIS.HTTP.Extensions
{
    public static class StringExtensions
    {
	public static string Capitalize(this string text)
	{
	    CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
	    TextInfo textInfo = cultureInfo.TextInfo;
	    string capitalizedText = textInfo.ToTitleCase(text.ToLower());
	    return capitalizedText;
	}
    }
}
