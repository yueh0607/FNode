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
        /// 节点类型
        /// </summary>
        public string UniqueName;

        /// <summary>
        /// 字段信息
        /// </summary>
        public string Fields;
    }
}
