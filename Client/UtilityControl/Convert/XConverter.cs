using System.Windows.Controls;
using Utility.Sys;

namespace UtilityControl.Convert
{
    public sealed class XConverter
    {
        public static BooleanToVisibilityConverter BooleanToVisibilityConverter
        {
            get { return Singleton<BooleanToVisibilityConverter>.GetInstance(); }
        }

        public static TrueToFalseConverter TrueToFalseConverter
        {
            get { return Singleton<TrueToFalseConverter>.GetInstance(); }
        }

        public static ThicknessToDoubleConverter ThicknessToDoubleConverter
        {
            get { return Singleton<ThicknessToDoubleConverter>.GetInstance(); }
        }
        public static BackgroundToForegroundConverter BackgroundToForegroundConverter
        {
            get { return Singleton<BackgroundToForegroundConverter>.GetInstance(); }
        }
        public static TreeViewMarginConverter TreeViewMarginConverter
        {
            get { return Singleton<TreeViewMarginConverter>.GetInstance(); }
        }

        public static PercentToAngleConverter PercentToAngleConverter
        {
            get { return Singleton<PercentToAngleConverter>.GetInstance(); }
        }

        public static TreeViewItemMarginLeftConverter TreeViewItemMarginLeftConverter
        {
            get { return Singleton<TreeViewItemMarginLeftConverter>.GetInstance(); }
        }
    }
}
