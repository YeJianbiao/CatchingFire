using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Utility.Contract
{
    public class PathContract
    {

        public static string DownloadPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download");

        //public static string 
    }
}
