using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNode.Editor
{
    public class GraphViewMenuItemAttribute : Attribute
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [JsonProperty("UniqueKey")]
        [JsonRequired]
        public string UniqueKey { get; private set; }


        /// <summary>
        /// 菜单项：Scene/Story 
        /// </summary>
        [JsonProperty("MenuItem")]
        [JsonRequired]
        public string MenuItem { get; private set; }

        /// <summary>
        /// 节点在图的归属(默认为0)
        /// </summary>
        [JsonProperty("Owners")]
        [JsonRequired]
        public int[] Owners { get; private set; }

        /// <summary>
        /// 路径根据"/"分割的部分
        /// </summary>

        [JsonIgnore]
        public string[] Parts { get; private set; }

        [JsonProperty("AssociationType")]
        [JsonRequired]
        internal Type AssociationType { get; private set; }


        public GraphViewMenuItemAttribute(string uniqueKey, string menuItem)
        {
            UniqueKey = uniqueKey;
            MenuItem = menuItem;
            Owners = new int[1] { 0 };
            Parts = MenuItem.Split('/');
        }
        public GraphViewMenuItemAttribute(string uniqueKey, string menuItem, params int[] belongs)
        {
            UniqueKey = uniqueKey;
            MenuItem = menuItem;
            Owners = belongs;
            Parts = MenuItem.Split('/');
        }

        [JsonConstructor] 
        internal GraphViewMenuItemAttribute(string uniqueKey, string menuItem, int[] owners, Type associationType)
             : this(uniqueKey, menuItem, owners)
        {
            AssociationType = associationType;
        }

        internal GraphViewMenuItemAttribute(GraphViewMenuItemAttribute otherAtt, Type associationType)
            : this(otherAtt.UniqueKey, otherAtt.MenuItem, otherAtt.Owners, associationType)
        {

        }

        private GraphViewMenuItemAttribute() { }

         
        public override string ToString()
        {
            return $"UniqueKey:{UniqueKey},MenuItem:{MenuItem},Owners:{string.Join('|', Owners)},AssociationType:{AssociationType.FullName}";
        }
    }
}
