/**
* 命名空间: CatchingFire.Area.Sys.ViewModel 
* 类    名： MenuViewModel
* 创 建 人：lenovo
* 创建时间：2018/8/19 3:13:25
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ApiService.Sys;
using GalaSoft.MvvmLight;
using UtilityControl.Model;

namespace CatchingFire.Area.Sys.ViewModel
{
    public class MenuViewModel:ViewModelBase
    {

        public MenuViewModel()
        {
            LoadMenu();
        }

        public ObservableCollection<TreeNode> Menus { get; set; }

        private async void LoadMenu()
        {
            var navMenus = await new MenuManager().GetMenu();
            if (!navMenus.IsSuccess)
            {
                return;
            }
           
            Menus = new ObservableCollection<TreeNode>();
            foreach (var menu in navMenus.Data.Where(o => string.IsNullOrEmpty(o.ParentCode)).OrderBy(o => o.Sequence))
            {
                var node = new TreeNode
                {
                    Code = menu.Code,
                    Name = menu.Name,
                    Tag = menu.Url,
                    Icon = menu.Icon,
                    NodeType = GetTreeNodeType(menu.Tag),
                    Nodes = new List<TreeNode>()
                };

                foreach (var subMenu in navMenus.Data.Where(o => o.ParentCode == menu.Code).OrderBy(o => o.Sequence))
                {
                    var subNode = new TreeNode
                    {
                        Code = subMenu.Code,
                        Name = subMenu.Name,
                        Tag = subMenu.Url,
                        Icon = subMenu.Icon,
                        NodeType = GetTreeNodeType(subMenu.Tag)
                    };

                    node.Nodes.Add(subNode);
                }
                Menus.Add(node);
            }
        }
        private TreeNodeType GetTreeNodeType(string str)
        {
            switch (str)
            {
                case "WPF_PAGE":
                    return TreeNodeType.Page;
                case "WPF_WINDOW":
                    return TreeNodeType.Window;
                case "WPF_DIALOG_WINDOW":
                    return TreeNodeType.DialogWindow;
                default:
                    return TreeNodeType.Page;
            }
        }
    }
}
