using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{
    /// <summary>
    /// 图数据
    /// </summary>
    public class SOGraphData : ScriptableObject
    {
        /// <summary>
        /// 节点
        /// </summary>
        public List<NodeData> nodes = new List<NodeData>();

        /// <summary>
        /// 连接
        /// </summary>
        public List<LinkData> links = new List<LinkData>();

        /// <summary>
        /// 视图信息
        /// </summary>
        public ViewData view = new ViewData();

        

    }
}
