using System;

namespace Dapper.Contrib.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ExplicitKeyAttribute : Attribute
    {
    }
}
