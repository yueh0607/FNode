using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FNode
{
    public class GraphViewMenuItemAttribute : Attribute
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string UniqueKey { get;private set; }


        /// <summary>
        /// 菜单项：Scene/Story 
        /// </summary>
        public string MenuItem { get; private set; }

        /// <summary>
        /// 觉得节点在图的归属(默认为0)
        /// </summary>
        public int[] Owner { get; private set; }

        /// <summary>
        /// 路径根据"/"分割的部分
        /// </summary>

        public string[] Parts { get; private set; }

        public GraphViewMenuItemAttribute(string uniqueKey,string menuItem,params int[] belongs)
        {
            UniqueKey = uniqueKey;
            MenuItem = menuItem;
            Owner = belongs;
            Parts = MenuItem.Split('/');
        }
    }
}
