/**
* 命名空间: Entity.Sys 
* 类    名： FileLog
* 创 建 人：lenovo
* 创建时间：2018/8/19 3:36:42
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System.Collections.Generic;

namespace Entity.Sys
{
    public class FileLog
    {

        public string Name { get; set; }

        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FileType Type { get; set; } 

        public IEnumerable<FileLog> Files { get; set; } 
    }

    public enum FileType
    {
        Folder,
        File
    }
}
