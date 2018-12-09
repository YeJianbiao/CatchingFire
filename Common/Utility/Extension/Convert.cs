using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Extension
{
    public static class Convert
    {

        public static string ToSafeString(this object value)
        {
            return Helper.Convert.ToString(value);
        }

        public static double ToDouble(this string value)
        {
            return Helper.Convert.ToDouble(value);
        }
    }
}
