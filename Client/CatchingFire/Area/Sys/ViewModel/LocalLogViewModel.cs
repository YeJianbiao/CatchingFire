/**
* 命名空间: CatchingFire.Area.Sys.ViewModel 
* 类    名： LocalLogViewModel
* 创 建 人：lenovo
* 创建时间：2018/8/20 22:28:12
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiService.Sys;
using CatchingFire.ViewModel;
using Entity.Sys;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Utility.Helper;
using FileType = Entity.Sys.FileType;
using MessageBox = UtilityControl.Control.MessageBox;

namespace CatchingFire.Area.Sys.ViewModel
{
    public class LocalLogViewModel:ViewModelBase
    {

        public LocalLogViewModel()
        {
            Init();
        }

        private async void Init()
        {
            try
            {
                var result = await new LogManager().GetLocalLog();
                if (result.IsSuccess)
                {
                    _fileLogs = result.Data.ToList();
                    FileLogs = new ObservableCollection<FileLog>();
                    foreach (var fileLog in result.Data)
                    {
                        FileLogs.Add(fileLog.TransReflection());
                    }
                }
                else
                {
                    MessageBox.Show("提示", result.Message, MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("提示", ex.Message, MessageBoxButton.OK);
            }

            IsLoading = false;
        }

        private string _currentFolder = string.Empty;

        private List<FileLog> _fileLogs;

        public bool IsLoading { get; set; } = true;

        public ObservableCollection<FileLog> FileLogs { get; set; }


        private int _pageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
                RaisePropertyChanged(() => PageCount);
                LogPages.Clear();
                for (int i = 0; i < _pageCount; i++)
                {
                    var pageModel = new PageModel
                    {
                        Index = i + 1,
                        IsSelected = i == 0
                    };
                    LogPages.Add(pageModel);
                }
            }
        }

        public int CurrentPage { get; set; } = 0;

        public ObservableCollection<PageModel> LogPages { get; set; } = new ObservableCollection<PageModel>();

        private void OpenFolder(FileLog fileLog)
        {
            if (fileLog.Type == FileType.File)
            {
                return;
            }
            _currentFolder = fileLog.Path;
            FileLogs.Clear();
            foreach (var log in fileLog.Files)
            {
                FileLogs.Add(log.TransReflection());
            }

        }

        private void OpenLog(FileLog fileLog)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Common.Instance.Parameter = fileLog.Path;
                    Common.Instance.LogType = "LocalLog";
                    Common.ControlHelper.OpenWindow("CatchingFire.Area.Sys.View.LogPreview", null, false);
                }));
            });
        }
        

        private void OpenLogFolder(FileLog fileLog)
        {
            System.Diagnostics.Process.Start("explorer", "/select,\"" + fileLog.Path + "\"");
        }

        private void Home()
        {
            if (string.IsNullOrEmpty(_currentFolder))
            {
                return;
            }
            _currentFolder = string.Empty;
            FileLogs.Clear();
            foreach (var log in _fileLogs)
            {
                FileLogs.Add(log.TransReflection());
            }
        }

        private FileLog GetCurrentPath(IEnumerable<FileLog> fileLogs)
        {
            if (fileLogs == null)
            {
                return null;
            }
            foreach (var fileLog in fileLogs)
            {
                if (fileLog.Path == _currentFolder)
                {
                    return fileLog;
                }
                var result = GetCurrentPath(fileLog.Files);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        private async void Refresh()
        {
            IsLoading = true;
            var result = await new LogManager().GetLocalLog();
            if (result.IsSuccess)
            {
                _fileLogs = result.Data.ToList();
                FileLogs = new ObservableCollection<FileLog>();

                var folder = GetCurrentPath(result.Data);
                var fileLogs = folder == null ? result.Data : folder.Files;

                foreach (var fileLog in fileLogs)
                {
                    FileLogs.Add(fileLog.TransReflection());
                }
                LogPages = new ObservableCollection<PageModel> { new PageModel { Index = 1, IsSelected = true } };
            }
            else
            {
                MessageBox.Show("提示", result.Message, MessageBoxButton.OK);
            }
            IsLoading = false;
        }

        private void Back()
        {
            if (string.IsNullOrEmpty(_currentFolder))
            {
                return;
            }

            foreach (var fileLog in _fileLogs.Where(fileLog => fileLog.Files != null && fileLog.Files.Count(o => o.Path == _currentFolder) == 1))
            {
                _currentFolder = fileLog.Path;
                FileLogs.Clear();
                foreach (var log in fileLog.Files)
                {
                    FileLogs.Add(log.TransReflection());
                }
                LogPages = new ObservableCollection<PageModel> { new PageModel { Index = 1, IsSelected = true } };
                return;
            }
            Home();
        }

        public ICommand OpenLogCommand => new RelayCommand<FileLog>(OpenLog);

        public ICommand OpenLogFolderCommand => new RelayCommand<FileLog>(OpenLogFolder);

        public ICommand OpenFolderCommand => new RelayCommand<FileLog>(OpenFolder);

        public ICommand HomeCommand => new RelayCommand(Home);

        public ICommand RefreshCommand => new RelayCommand(Refresh);

        public ICommand BackCommand => new RelayCommand(Back);

    }
}
