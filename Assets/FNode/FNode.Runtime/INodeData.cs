using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Runtime
{
    public interface INodeData
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public string GUID { get; }


        /// <summary>
        /// 节点唯一标识
        /// </summary>
        public string UniqueName { get; }
    }
}
