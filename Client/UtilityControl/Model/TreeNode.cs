using System.Collections;
using System.Collections.Generic;

namespace UtilityControl.Model
{
    public class TreeNode
    {
        #region Property

        /// <summary>
        /// 唯一值
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 显示的文本值
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 暂时只对主页菜单有效，打开窗口类型：Page,Window,DialogWindow
        /// </summary>
        public TreeNodeType NodeType { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool? Checked { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpand { get; set; }

        /// <summary>
        /// 节点图标：相对路径
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 子节点，默认null
        /// </summary>
        public IList<TreeNode> Nodes { get; set; }

        /// <summary>
        /// 该节点数据项，默认null
        /// </summary>
        public virtual IList ItemSource { get; set; }


        /// <summary>
        /// 视图控件，只有当ViewType=UserControl时才有效
        /// </summary>
        public string ViewControl { get; set; }

        #endregion

        #region 构造函数（初始化）

        /// <summary>
        ///  NodeX-构造函数（初始化）
        /// </summary>
        public TreeNode()
        {
            Name = string.Empty;
            Icon = string.Empty;
            Checked = false;
        }

        #endregion
    }

    public enum TreeNodeType
    {
        Page,
        Window,
        DialogWindow

    }

}
