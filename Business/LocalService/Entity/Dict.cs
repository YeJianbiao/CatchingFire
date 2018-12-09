/**
* 命名空间: LocalService.Entity 
* 类    名： Dict
* 创 建 人：lenovo
* 创建时间：2018/8/12 18:16:14
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
    [Table("dict")]
    public class Dict
    {
        //[PrimaryKey,Column("id")]
        //public string Id { get; set; } = Guid.NewGuid().ToString();

        [PrimaryKey,Column("code"),MaxLength(50)]
        public string Code { get; set; }

        [Column("value"), MaxLength(250)]
        public string Value { get; set; }

        [Column("value_1"), MaxLength(250)]
        public string Value_1 { get; set; }

        [Column("value_2"), MaxLength(250)]
        public string Value_2 { get; set; }

        [Column("value_3"), MaxLength(250)]
        public string Value_3 { get; set; }

    }
}
