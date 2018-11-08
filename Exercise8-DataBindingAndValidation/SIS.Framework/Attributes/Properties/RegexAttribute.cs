using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SIS.Framework.Attributes.Properties
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RegexAttribute : ValidationAttribute
    {
	private readonly string pattern;

	public RegexAttribute(string pattern)
	{
	    this.pattern = pattern;
	}

	//TODO: USE public ValidationResult IsValid(object value, ValidationContext validationContext),
	// OR public override bool IsValid(object value) WITH BUILT-IN ValidationContext INSTEAD ???
	public override bool IsValid(object value)
	{
	    return Regex.IsMatch(value.ToString(), pattern);
	}
    }
}
