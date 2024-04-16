using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;
using UnityEngine;

namespace FNode.Editor
{
    /// <summary>
    /// 用于存储单个节点的信息
    /// </summary>
    [System.Serializable]
    public class GraphViewMenuItem
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string UniqueKey;

        /// <summary>
        /// 菜单项：Scene/Story 
        /// </summary>
        public string MenuItem;

        /// <summary>
        /// 觉得节点在图的归属(默认为0)
        /// </summary>
        public int[] Owner;

        /// <summary>
        /// 路径根据"/"分割的部分
        /// </summary>
        public string[] Parts;

        /// <summary>
        /// 对应的节点类型
        /// </summary>
        public Type NodeType;

        [JsonConstructor]
        public GraphViewMenuItem(string uniqueKey, string menuItem, int[] owner, string[] parts, Type nodeType)
        {
            UniqueKey = uniqueKey;
            MenuItem = menuItem;
            NodeType = nodeType;
            Owner = owner;
            Parts = parts;
        }


        public GraphViewMenuItem(GraphViewMenuItemAttribute graphViewMenuItemAtt, Type nodeType)
        {
            UniqueKey = graphViewMenuItemAtt.UniqueKey;
            MenuItem = graphViewMenuItemAtt.MenuItem;
            Owner = graphViewMenuItemAtt.Owner;
            Parts = graphViewMenuItemAtt.Parts;
            NodeType = nodeType;
        }


        public override string ToString()
        {
            return $"UniqueKey:{UniqueKey} UniqueKey:{MenuItem} Owner:{string.Join(",", Owner)} Parts:{string.Join(",", Parts)} NodeType{NodeType.Name}";
        }

    }
}
