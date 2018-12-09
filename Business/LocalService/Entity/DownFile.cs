/**
* 命名空间: LocalService.Entity 
* 类    名： DownFile
* 创 建 人：lenovo
* 创建时间：2018/8/19 23:07:27
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System;
using LocalService.SQLite;

namespace LocalService.Entity
{
    [Table("down_file")]
    public class DownFile
    {
        [PrimaryKey,Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("file_name")]
        public string FileName { get; set; }

        [Column("down_path")]
        public string DownPath { get; set; }


    }
}
