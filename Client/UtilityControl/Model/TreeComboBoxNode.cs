using System.Collections.Generic;

namespace UtilityControl.Model
{
    public class TreeComboBoxNode
    {
        public string Code { get; set; }

        public string Name { get; set; }

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
        public IList<TreeComboBoxNode> Nodes { get; set; }
    }
}
