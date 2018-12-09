/**
* 命名空间: LocalService.Manager 
* 类    名： CommonMenuManager
* 创 建 人：lenovo
* 创建时间：2018/8/14 20:28:36
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System.Linq;
using LocalService.Entity;
using LocalService.SQLite;

namespace LocalService.Manager
{
    public class CommonMenuManager
    {

        public void AddMenuCount(string userNo,string menuCode)
        {
            var menu =
                SQLiteHelper.DB.Table<CommonMenu>().FirstOrDefault(o => o.UserNo == userNo && o.MenuCode == menuCode);
            if (menu == null)
            {
                menu = new CommonMenu
                {
                    UserNo = userNo,
                    MenuCode = menuCode,
                    ClickCount = 1
                };
            }
            else
            {
                menu.ClickCount = menu.ClickCount + 1;
            }
            SQLiteHelper.DB.InsertOrReplace(menu);
        }

    }
}
