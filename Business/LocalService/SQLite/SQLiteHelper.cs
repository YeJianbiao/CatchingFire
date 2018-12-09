/**
* 命名空间: LocalService.SQLite 
* 类    名： SQLiteHelper
* 创 建 人：lenovo
* 创建时间：2018/8/12 18:11:56
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System;
using System.IO;

namespace LocalService.SQLite
{
    public class SQLiteHelper : SQLiteConnection
    {

        private const string DB_Path = "SQLite_DB";

        private const string DB_Name = "CatchingFire.db";

        //private string DB_Full_Path = AppDomain.CurrentDomain.BaseDirectory + "//" + DB_Path + "//" + DB_Name;

        private const int Version = 2;

        public static SQLiteHelper DB
        {
            get
            {
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SQLite_DB")))
                {
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SQLite_DB"));
                }
                return new SQLiteHelper();
            }
        }

        private SQLiteHelper() : base(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DB_Path, DB_Name), true)
        {
            Trace = true;
            OnUpgrade();
        }

        private void OnCreate()
        {
            CreateTable<Entity.User>();
            CreateTable<Entity.Dict>();
            CreateTable<Entity.CommonMenu>();
        }

        private void OnDrop()
        {
            DropTable<Entity.User> ();
            DropTable<Entity.Dict>();
            DropTable<Entity.CommonMenu>();
        }

        private void OnUpgrade()
        {
            var currentVersion = ExecuteScalar<int>("PRAGMA user_version");
            if (Version > currentVersion)
            {
                OnDrop();
                Execute($"PRAGMA user_version={Version}");
            }
            OnCreate();

        }

    }
}
