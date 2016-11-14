using System;

namespace Dapper.Contrib.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class ExplicitKeyAttribute : Attribute
    {
    }
}
