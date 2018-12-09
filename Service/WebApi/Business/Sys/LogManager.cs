using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entity.Sys;

namespace WebApi.Business.Sys
{
    /// <summary>
    /// 
    /// </summary>
    public class LogManager:BaseManager<string>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FileLog>> GetFileLogs()
        {
           return await Task.Run(() =>
             {
                 string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                 var dirs = GetAllDirectories(logPath);
                 return dirs;
             });
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