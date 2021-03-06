﻿using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class RedirectResult : HttpResponse
    {
	public RedirectResult(string location) : base(HttpResponseStatusCode.SeeOther)
	{
	    Headers.Add(new HttpHeader(GlobalConstants.LocationHeaderKey, location));
	}
    }
}
