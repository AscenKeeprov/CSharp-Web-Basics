﻿using System;
using SIS.HTTP.Enumerations;

namespace SIS.Framework.Attributes.Methods
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class HttpMethodAttribute : Attribute
    {
	public abstract bool IsValid(HttpRequestMethod requestMethod);
    }
}
