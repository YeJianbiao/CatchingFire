/**
* 命名空间: LocalService.Entity 
* 类    名： CommonMenu
* 创 建 人：lenovo
* 创建时间：2018/8/14 20:02:47
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
    [Table("common_menu")]
    public class CommonMenu
    {
        [PrimaryKey,Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("user_no")]
        public string UserNo { get; set; }

        [Column("menu_code")]
        public string MenuCode { get; set; }

        [Column("click_count")]
        public int ClickCount { get; set; }

    }
}
