using System;

namespace SIS.HTTP.Exceptions
{
    public class InvalidObjectValueException : Exception
    {
	private new const string Message = "Invalid {0}: {1}";

	public InvalidObjectValueException(string objectName, string invalidValue)
	    : base(string.Format(Message, objectName, invalidValue)) { }
    }
}
