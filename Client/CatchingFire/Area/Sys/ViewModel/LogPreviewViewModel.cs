/**
* 命名空间: CatchingFire.Area.Sys.ViewModel 
* 类    名： LogPreviewViewModel
* 创 建 人：lenovo
* 创建时间：2018/8/29 21:37:21
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System;
using System.IO;
using System.Text;
using System.Windows;
using ApiService.ApiAddress;
using GalaSoft.MvvmLight;
using Utility.Http;
using MessageBox = UtilityControl.Control.MessageBox;

namespace CatchingFire.Area.Sys.ViewModel
{
    public class LogPreviewViewModel:ViewModelBase
    {

        public LogPreviewViewModel()
        {
            Init();    
        }

        public string Value { get; set; }

        private async void Init()
        {
            try
            {
                if (Common.Instance.LogType == "LocalLog")
                {
                    var bytes = File.ReadAllBytes(Common.Instance.Parameter + "");
                    Value += Encoding.Default.GetString(bytes);
                    return;
                }
                var url =BaseApi.GetDomain()+SysApi.Download + Common.Instance.Parameter;
                using (var httpClient = HttpClientUtil.CreateHttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    var contentType = response.Content.Headers.ContentType;
                    if (string.IsNullOrEmpty(contentType.CharSet))
                    {
                        contentType.CharSet = "utf-8";
                    }
                    long contentLength =-1;
                    if (response.Content.Headers.ContentLength != null)
                    {
                        contentLength = response.Content.Headers.ContentLength.Value;
                    }

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var readLength = 10*1024;
                        if (readLength != -1 && contentLength < readLength)
                        {
                            readLength = (int)contentLength;
                        }
                        var bytes = new byte[readLength];
                        while (((stream.Read(bytes,0,readLength)))>0)
                        {
                            Value +=Encoding.Default.GetString(bytes);
                            contentLength -= readLength;
                            if (contentLength <= 0)
                            {
                                break;
                            }
                            readLength = Math.Min((int)contentLength, readLength);
                            bytes = new byte[readLength];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误", ex.Message, MessageBoxButton.OK);
                Utility.Logger.LogHelper.Error("下载文件出错", ex);
            }

        }

    }
}
