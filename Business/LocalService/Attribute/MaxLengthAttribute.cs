using System;

namespace LocalService.Attribute
{

    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLengthAttribute : System.Attribute
    {
        public int Value { get; private set; }

        public MaxLengthAttribute(int length)
        {
            Value = length;
        }
    }
}
