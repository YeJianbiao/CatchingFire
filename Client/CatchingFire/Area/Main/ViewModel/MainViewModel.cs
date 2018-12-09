using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using ApiService.Sys;
using CatchingFire.Common;
using Entity.Sys;
using LocalService.Entity;
using LocalService.Manager;
using LocalService.SQLite;
using Utility.Helper;
using UtilityControl.Common;
using UtilityControl.Control;
using UtilityControl.Model;

namespace CatchingFire.Area.Main.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            LoadMenu();
        }

        #region 数据初始化

        public string LoginUser { get; set; } = Instance.UserName;

        public ObservableCollection<TreeNode> Menus { get; set; }

        public ObservableCollection<TreeNode> CommonUsedMenus { get; set; }

        private async void LoadMenu()
        {
            var navMenus = await new MenuManager().GetMenu(Instance.UserName);
            if (!navMenus.IsSuccess)
            {
                return;
            }
            LoadCommonUsedMenus(navMenus.Data);
            Menus = new ObservableCollection<TreeNode>();
            foreach (var menu in navMenus.Data.Where(o => string.IsNullOrEmpty(o.ParentCode)).OrderBy(o => o.Sequence))
            {
                var node = new TreeNode
                {
                    Code = menu.Code,
                    Name = menu.Name,
                    Tag = menu.Url,
                    Icon = menu.Icon,
                    NodeType = GetTreeNodeType(menu.Tag),
                    Nodes = new List<TreeNode>()
                };

                foreach (var subMenu in navMenus.Data.Where(o => o.ParentCode == menu.Code).OrderBy(o => o.Sequence))
                {
                    var subNode = new TreeNode
                    {
                        Code = subMenu.Code,
                        Name = subMenu.Name,
                        Tag = subMenu.Url,
                        Icon = subMenu.Icon,
                        NodeType = GetTreeNodeType(subMenu.Tag)
                    };

                    node.Nodes.Add(subNode);
                }
                Menus.Add(node);
            }
        }

        private void LoadCommonUsedMenus(IEnumerable<Menu> menus)
        {
            if (menus == null) { return; }
            CommonUsedMenus = new ObservableCollection<TreeNode>();
            var commonMenus = SQLiteHelper.DB.Table<CommonMenu>().Where(o => o.UserNo == Instance.UserName).OrderByDescending(o => o.ClickCount);
            if (commonMenus != null)
            {
                foreach (var commonMenu in commonMenus)
                {
                    if (CommonUsedMenus.Count >= 10) { return; }
                    var menu = menus.FirstOrDefault(o => o.Code == commonMenu.MenuCode);
                    if (menu != null)
                    {
                        var node = new TreeNode
                        {
                            Code = menu.Code,
                            Name = menu.Name,
                            Tag = menu.Url,
                            Icon = menu.Icon,
                            NodeType = GetTreeNodeType(menu.Tag),
                            Checked = CommonUsedMenus.Count == 0
                        };

                        CommonUsedMenus.Add(node);
                    }
                }
            }
            foreach (var menu in menus.Where(o => !string.IsNullOrEmpty(o.Url) && o.IsCommonUse).OrderBy(o => o.Sequence))
            {
                if (CommonUsedMenus.Count >= 10) { return; }
                var node = new TreeNode
                {
                    Code = menu.Code,
                    Name = menu.Name,
                    Tag = menu.Url,
                    Icon = menu.Icon,
                    NodeType = GetTreeNodeType(menu.Tag),
                    Checked = CommonUsedMenus.Count == 0
                };
                if (CommonUsedMenus.All(o => o.Code != node.Code))
                {
                    CommonUsedMenus.Add(node);
                }
            }
        }

        private TreeNodeType GetTreeNodeType(string str)
        {
            switch (str)
            {
                case "WPF_PAGE":
                    return TreeNodeType.Page;
                case "WPF_WINDOW":
                    return TreeNodeType.Window;
                case "WPF_DIALOG_WINDOW":
                    return TreeNodeType.DialogWindow;
                default:
                    return TreeNodeType.Page;
            }
        }

        #endregion

        #region 属性

        public Uri PageUri { get; set; }

        #endregion

        private void SelectedMenu(TreeNode node)
        {
            if (string.IsNullOrEmpty(node?.Tag) || node.Tag == PageUri?.OriginalString)
            {
                return;
            }
            new CommonMenuManager().AddMenuCount(Instance.UserName, node.Code);
            switch (node.NodeType)
            {
                case TreeNodeType.DialogWindow:
                    Common.ControlHelper.OpenWindow(node.Tag);
                    break;
                case TreeNodeType.Window:
                    Task.Run(() =>
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Common.ControlHelper.OpenWindow(node.Tag, null, false);
                        }));
                    });
                    break;
                default:
                    PageUri = new Uri(node.Tag, UriKind.RelativeOrAbsolute);
                    break;
            }
        }

        private void OpenDownloadList()
        {
            var popupMenu = Application.Current.MainWindow.GetChild<PopupExtession>("popupMenu");
            popupMenu.IsOpen = false;
            var downloadList = Application.Current.Windows.Cast<Window>().FirstOrDefault(o => o.GetType() == typeof(Sys.View.DownloadList));
            if (downloadList != null)
            {
                downloadList.Activate();
                return;
            }
            //Task.Run(() =>
            //{
            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            Common.ControlHelper.OpenWindow("CatchingFire.Area.Sys.View.DownloadList", null, false);
            //    }));
            //});
        }

        private void OpenMessage()
        {
            var message = Application.Current.Windows.Cast<Window>().FirstOrDefault(o => o.GetType() == typeof(Message.View.Message));
            if (message != null)
            {
                message.Activate();
                return;
            }
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Common.ControlHelper.OpenWindow("CatchingFire.Area.Message.View.Message");
                }));
            });
        }

        private void OpenChat()
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var process = WinApiManager.RunningInstance("CatchingFire");
                    if (process != null)
                    {
                        WinApiManager.HandleRunningInstance(process);
                        return;
                    }
                    Common.ControlHelper.OpenWindow("Chat.MainWindow", null, false, false, "Chat");
                }));
            });
        }

        #region ICommand

        public ICommand SelectedMenuCommand => new RelayCommand<TreeNode>(SelectedMenu);

        public ICommand OpenMessageCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OpenMessage);

        public ICommand DownloadListCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OpenDownloadList);

        public ICommand OpenChatCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OpenChat);

        #endregion

        #region 界面设置

        public void SwitchToChinese()
        {
            Application.Current.Resources.MergedDictionaries[1] = new ResourceDictionary { Source = new Uri("pack://application:,,,/UtilityControl;component/Resource/Language/Language_zh-CN.xaml", UriKind.RelativeOrAbsolute) };
            foreach (var window in Application.Current.Windows)
            {
                (window as DependencyObject).LanguageTranslation();
            }
            SQLiteHelper.DB.InsertOrReplace(new LocalService.Entity.Dict { Code = "Language", Value = "zh-CN" });
            var popupMenu = Application.Current.MainWindow.GetChild<PopupExtession>("popupMenu");
            popupMenu.IsOpen = false;
        }

        private int _theme = 0;
        public void ChangTheme()
        {
            if (_theme == 0)
            {
                Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary
                {
                    Source =
                        new Uri("pack://application:,,,/UtilityControl;component/Style/Themes/Blue.xaml",
                            UriKind.RelativeOrAbsolute)
                };
                _theme = 1;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary
                {
                    Source =
                        new Uri("pack://application:,,,/UtilityControl;component/Style/Themes/Default.xaml",
                            UriKind.RelativeOrAbsolute)
                };
                _theme = 0;
            }
            var popupMenu = Application.Current.MainWindow.GetChild<PopupExtession>("popupMenu");
            popupMenu.IsOpen = false;
        }

        public void SwitchToEnglish()
        {
            Application.Current.Resources.MergedDictionaries[1] = new ResourceDictionary { Source = new Uri("pack://application:,,,/UtilityControl;component/Resource/Language/Language_en.xaml", UriKind.RelativeOrAbsolute) };
            foreach (var window in Application.Current.Windows)
            {
                (window as DependencyObject).LanguageTranslation();
            }
            LocalService.SQLite.SQLiteHelper.DB.InsertOrReplace(new LocalService.Entity.Dict { Code = "Language", Value = "en" });
            var popupMenu = Application.Current.MainWindow.GetChild<PopupExtession>("popupMenu");
            popupMenu.IsOpen = false;
        }

        private void Setting()
        {
            var popupMenu = Application.Current.MainWindow.GetChild<PopupExtession>("popupMenu");
            var btnSet = Application.Current.MainWindow.GetChild<UButton>("btnSet");
            if (popupMenu == null || btnSet == null) { return; }
            popupMenu.PlacementTarget = btnSet;
            popupMenu.IsOpen = true;
        }

        private void OpenSetting()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Common.ControlHelper.OpenWindow("CatchingFire.Area.Setting.View.Main");
            }));
        }

        private void ExitSystem()
        {
            var result = UtilityControl.Control.MessageBox.Show("提示", "确定要退出系统吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }
            Application.Current.Shutdown();
        }

        private void UnCompleted()
        {
            UtilityControl.Control.MessageBox.Show("提示", "建设中...", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        public ICommand ExitSystemCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(ExitSystem);

        public ICommand SettingCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(Setting);

        public ICommand OpenSettingCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OpenSetting);

        public ICommand SwitchToEnglishCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(SwitchToEnglish);

        public ICommand SwitchToChineseCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(SwitchToChinese);

        public ICommand ChangThemeCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(ChangTheme);

        public ICommand UnCompletedCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(UnCompleted);
        #endregion


        
    }

}
