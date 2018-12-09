
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using UtilityControl.Control;
using RelayCommand = GalaSoft.MvvmLight.CommandWpf.RelayCommand;

namespace CatchingFire.Area.TechnologySummarize.ViewModel
{
    public class ControlsViewModel : ViewModelBase
    {

        public ControlsViewModel()
        {
            RefreshProgress();
        }

        public bool IsLoading { get; set; }

        public string KeyWords { get; set; } = "数";

        public RelayCommand OpenLoadingDialogCommand => new RelayCommand(OpenLoadingDialog);

        private void OpenLoadingDialog()
        {
            ULoadingDialog.Show("正在加载中，请稍后...");
            Wait(5);
        }

        private async void Wait(int second)
        {
            await Task.Delay(second * 1000);
            ULoadingDialog.Close();
        }

        public RelayCommand OpenToastCommand => new RelayCommand(OpenToast);

        private int _toastIndex = 0;
        private void OpenToast()
        {
            if (_toastIndex == 0)
            {
                Toast.Show("保存成功", ToastType.Success);
            }
            else if (_toastIndex == 1)
            {
                Toast.Show("保存失败", ToastType.Error);
            }
            else if (_toastIndex == 2)
            {
                Toast.Show("信息提示", ToastType.Info);
            }
            else
            {
                Toast.Show("Warn Warn Warn Warn Warn Warn Warn Warn Warn", ToastType.Warn);
                _toastIndex = -1;
            }

            _toastIndex += 1;

        }

        public int Progress { get; set; }

        private async Task RefreshProgress()
        {
            if (Progress >= 100)
            {
                Progress = 0;
            }
            else
            {
                Progress++;
            }
            await Task.Delay(200);
            await RefreshProgress();
        }

        private Tuple<string, object> dsf(string text)
        {
            return new Tuple<string, object>(text, null);
        }

        public Func<string, Tuple<string, object>> FuzzySearch => searchText =>
        {
            object result= new[] { $"{searchText}搜索数据1....", $"{searchText}搜索数据2....", $"{searchText}搜索数据3....", $"{searchText}搜索数据4....", $"{searchText}搜索数据5...." };
           
            return new Tuple<string, object>(searchText, result);
        };

        public ICommand SearchCommand => new RelayCommand((() =>
        {
            Toast.Show("Search",ToastType.Info);
        }));

    }

}
