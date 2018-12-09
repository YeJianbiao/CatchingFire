using System;

namespace LocalService.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CollationAttribute : System.Attribute
    {
        public string Value { get; private set; }

        public CollationAttribute(string collation)
        {
            Value = collation;
        }
    }
}
