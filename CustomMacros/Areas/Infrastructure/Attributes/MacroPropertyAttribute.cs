using System;

namespace CustomMacros.Areas.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MacroPropertyAttribute : Attribute
    {
    }
}