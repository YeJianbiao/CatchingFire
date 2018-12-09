/**
* 命名空间: ApiService.Sys 
* 类    名： LogManager
* 创 建 人：lenovo
* 创建时间：2018/8/19 3:30:51
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiService.ApiAddress;
using ApiService.Http;
using Entity.Base;
using Entity.Sys;

namespace ApiService.Sys
{
    public class LogManager
    {

        public async Task<BaseResult<IEnumerable<FileLog>>> GetFileLog()
        {
            var url = SysApi.LogApi;
            var response = await HttpClientUtil.Instance.GetAsync(url);
            return Utils.HttpConvert<IEnumerable<FileLog>>.ConvertHttpResponse(response, "Success", "获取日志信息失败");

        }

        public async Task<BaseResult<IEnumerable<FileLog>>> GetLocalLog()
        {
            return await Task.Run((() =>
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                var dirs = GetAllDirectories(logPath);
                return new BaseResult<IEnumerable<FileLog>>(dirs,true,"","");
            }));
        }

        private static IEnumerable<FileLog> GetAllDirectories(string path)
        {
            var theFolder = new DirectoryInfo(path);
            var directories = theFolder.GetDirectories();
            //DirectoryInfo[] directories = Directory.GetDirectories(path);
            var fileLogs = directories.Select(dir => new FileLog()
            {
                Name = dir.Name,
                Path = dir.FullName,
                Type = FileType.Folder,
                Files = GetAllDirectories(dir.FullName)
            }).ToList();
            var files = Directory.GetFiles(path);
            fileLogs.AddRange(files.Select(file => new FileLog()
            {
                Name = Path.GetFileName(file),
                Path = file,
                Type = FileType.File,
            }));
            return fileLogs;
        }

    }
}
