using System;
using UnityEngine;

namespace FNode.Editor
{
    [Serializable]
    public class NodeData
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public string GUID;

        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// 节点唯一标识
        /// </summary>
        public string UniqueName;

        /// <summary>
        /// 节点特有字段信息
        /// </summary>
        public string Fields;
    }
}
