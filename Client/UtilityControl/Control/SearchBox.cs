using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UtilityControl.Control
{
    public class SearchBox:TextBox
    {

        static SearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UButton), new FrameworkPropertyMetadata(typeof(SearchBox)));
        }



    }
}
