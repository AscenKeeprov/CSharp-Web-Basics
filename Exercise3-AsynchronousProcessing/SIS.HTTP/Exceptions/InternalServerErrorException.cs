using System;
using System.Net;

namespace SIS.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
	public const HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;

	public override string Message => "The Server has encountered an error.";
    }
}
