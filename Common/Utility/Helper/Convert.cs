using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Helper
{
    public class Convert
    {

        public static string ToString(object value)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public static double ToDouble(string value)
        {
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            throw new InvalidCastException("不能将字符串\"" + value + "\"转换为double数字。");
        }

    }
}
