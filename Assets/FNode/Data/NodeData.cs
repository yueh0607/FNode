using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FNode.Data
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
