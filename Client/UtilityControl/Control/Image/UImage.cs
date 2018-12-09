using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UtilityControl.Control.Image
{
    public class UImage : System.Windows.Controls.Image
    {

        #region 注册属性、事件

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof (string), typeof (UImage), new PropertyMetadata(default(string)));

        public string ImageSource
        {
            get { return (string) GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty AllowsAnimationProperty = DependencyProperty.Register(
            "AllowsAnimation", typeof (bool), typeof (UImage), new PropertyMetadata(default(bool)));

        public bool AllowsAnimation
        {
            get { return (bool) GetValue(AllowsAnimationProperty); }
            set { SetValue(AllowsAnimationProperty, value); }
        }

        public static readonly DependencyProperty AllowsAsyncProperty = DependencyProperty.Register(
            "AllowsAsync", typeof (bool), typeof (UImage), new PropertyMetadata(default(bool)));

        public bool AllowsAsync
        {
            get { return (bool) GetValue(AllowsAsyncProperty); }
            set { SetValue(AllowsAsyncProperty, value); }
        }

        public static readonly DependencyProperty AllowsCacheProperty = DependencyProperty.Register(
            "AllowsCache", typeof (bool), typeof (UImage), new PropertyMetadata(default(bool)));

        public bool AllowsCache
        {
            get { return (bool) GetValue(AllowsCacheProperty); }
            set { SetValue(AllowsCacheProperty, value); }
        }

        public static readonly DependencyProperty CacheTimeProperty = DependencyProperty.Register(
            "CacheTime", typeof (int), typeof (UImage), new PropertyMetadata(default(int)));

        public int CacheTime
        {
            get { return (int) GetValue(CacheTimeProperty); }
            set { SetValue(CacheTimeProperty, value); }
        }

        #endregion

    }
}
