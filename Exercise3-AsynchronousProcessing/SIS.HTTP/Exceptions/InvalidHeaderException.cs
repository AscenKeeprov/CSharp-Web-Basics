using System;

namespace SIS.HTTP.Exceptions
{
    public class InvalidHeaderException : Exception
    {
	private new const string Message = "Invalid header: {0}";

	public InvalidHeaderException(string headerKey)
	    : base(string.Format(Message, headerKey)) { }
    }
}
