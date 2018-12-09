/**
* 命名空间: LocalService.Entity 
* 类    名： User
* 创 建 人：lenovo
* 创建时间：2018/8/12 21:52:59
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using LocalService.SQLite;

namespace LocalService.Entity
{
    [Table("user")]
    public class User
    {

        [PrimaryKey, Column("name"), MaxLength(50)]
        public string Name { get; set; }

        [Column("pwd"), MaxLength(50)]
        public string Pwd { get; set; }

        [Column("isDefault")]
        public bool IsDefault { get; set; }

    }
}
