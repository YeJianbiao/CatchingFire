/**
* 命名空间: ApiService.Sys 
* 类    名： MenuManager
* 创 建 人：lenovo
* 创建时间：2018/8/12 18:40:47
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using ApiService.ApiAddress;
using ApiService.Http;
using Entity.Base;
using Entity.Sys;

namespace ApiService.Sys
{
    public class MenuManager
    {
        public async Task<BaseResult<IEnumerable<Menu>>> GetMenu(string userName)
        {
            var url = SysApi.UserMenuApi + userName;
            try
            {
                var response = await HttpClientUtil.Instance.GetAsync(url);
                return Utils.HttpConvert<IEnumerable<Menu>>.ConvertHttpResponse(response, "Success", "获取导航菜单失败");
            }
            catch
            {
                //return Utils.HttpConvert<IEnumerable<Menu>>.ConvertHttpResponse(response, "Success", "获取导航菜单失败");
            }
            return new BaseResult<IEnumerable<Menu>>(GetLoadMenu(), true, "", null);
            //return new BaseResult<IEnumerable<Menu>>(null, false, "连接服务器错误！", null);
            //return Utils.HttpConvert<IEnumerable<Menu>>.ConvertHttpResponse(response, "Success", "获取导航菜单失败");

        }

        public async Task<BaseResult<IEnumerable<Menu>>> GetMenu()
        {
            var url = SysApi.MenuApi;
            try
            {
                var response = await HttpClientUtil.Instance.GetAsync(url);
                return Utils.HttpConvert<IEnumerable<Menu>>.ConvertHttpResponse(response, "Success", "获取导航菜单失败");
            }
            catch 
            {
               
            }
            return new BaseResult<IEnumerable<Menu>>(GetLoadMenu(), true, "", null);

        }

        private List<Menu> GetLoadMenu()
        {
            var list = new List<Menu>();
            var node1 = new Menu()
            {
                Code = "System_Manager",
                Name = "系统设置",
                NameEn = "System Setting",
                Icon = "",
                ParentCode = "",
                Sequence = 1,
                Url = "",
                Tag = "WPF_PAGE",
                IsCommonUse = false,
                Description = null,
                status = "1"
            };

            var node2 = new Menu()
            {
                Code = "Other_Icon",
                Name = "图标",
                NameEn = "Icon",
                Icon = "",
                ParentCode = "Other",
                Sequence = 30,
                Url = "pack://application:,,,/CatchingFire;component/Area/FontTest/View/FontTest.xaml",
                Tag = "WPF_PAGE",
                IsCommonUse = true,
            };

            var node3 = new Menu()
            {
                Code = "System_User",
                Name = "账号管理",
                NameEn = "System User",
                Icon = "",
                ParentCode = "System_Manager",
                Sequence = 20,
                Url = "pack://application:,,,/CatchingFire;component/Area/Sys/View/User.xaml",
                Tag = "WPF_PAGE",
                IsCommonUse = false
            };

            var node4 = new Menu()
            {
                Code = "Other",
                Name = "其它",
                NameEn = "Other",
                Icon = "",
                ParentCode = "",
                Sequence = 10,
                Url = "",
                Tag = "WPF_PAGE",
                IsCommonUse = true
            };

            var node5 = new Menu()
            {
                Code = "Other_Control",
                Name = "控件",
                NameEn = "Control",
                Icon = "",
                ParentCode = "Other",
                Sequence = 10,
                Url = "pack://application:,,,/CatchingFire;component/Area/TechnologySummarize/View/Controls.xaml",
                Tag = "WPF_PAGE",
                IsCommonUse = true,
            };

            var node6 = new Menu()
            {
                Code = "System_Navigation",
                Name = "导航菜单",
                NameEn = "Navigation",
                Icon = "",
                ParentCode = "System_Manager",
                Sequence = 10,
                Url = "pack://application:,,,/CatchingFire;component/Area/Sys/View/Menu.xaml",
                Tag = "WPF_PAGE",
                IsCommonUse = false
            };

            var node7 = new Menu()
            {
                Code = "System_Log",
                Name = "日志记录",
                NameEn = "Log",
                Icon = "",
                ParentCode = "System_Manager",
                Sequence = 30,
                Url = "pack://application:,,,/CatchingFire;component/Area/Sys/View/Log.xaml",
                Tag = "WPF_PAGE",
                IsCommonUse = false
            };


            list.Add(node1);
            list.Add(node2);
            list.Add(node3);
            list.Add(node4);
            list.Add(node5);
            list.Add(node6);
            list.Add(node7);
            return list;
        }

    }
}
