using System;
using System.IO;
using System.Windows;
using Utility.Helper;
using Utility.Logger;

namespace CatchingFire
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {

        [STAThread]
        public void AppMain()
        {
            var process = WinApiManager.RunningInstance("CatchingFire");
            if (process != null)
            {
                WinApiManager.HandleRunningInstance(process);
                Current.Shutdown();
                return;
            }
            App app = new App();
            app.Run();

        }

        private App()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            StartupUri = new Uri("Area/Account/View/Login.xaml", UriKind.Relative);
            Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LogHelper.SetConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logger", "log4net.config"));
            LogHelper.Info("程序开始启动...");

        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (!e.Handled)
            {
                UtilityControl.Control.MessageBox.Show("错误", e.Exception.Message, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                LogHelper.Fatal(e.Exception.Message, e.Exception);
                e.Handled = true;
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                throw (Exception)e.ExceptionObject;
            });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var lang = LocalService.SQLite.SQLiteHelper.DB.Find<LocalService.Entity.Dict>(o => o.Code == "Language");

            Current.Resources.MergedDictionaries.Add(lang?.Value == "en"
                ? new ResourceDictionary
                {
                    Source =
                        new Uri("pack://application:,,,/UtilityControl;component/Resource/Language/Language_en.xaml",
                            UriKind.RelativeOrAbsolute)
                }
                : new ResourceDictionary
                {
                    Source =
                        new Uri(
                            "pack://application:,,,/UtilityControl;component/Resource/Language/Language_zh-CN.xaml",
                            UriKind.RelativeOrAbsolute)
                });
        }


    }
}
