using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Runtime
{
    public class RuntimeLinkData
    {
        /// <summary>
        /// 起始节点
        /// </summary>
        public string FromGUID;
        /// <summary>
        /// 目标节点
        /// </summary>
        public string ToGUID;
        /// <summary>
        /// 目标端口
        /// </summary>
        public string ToPort;
        /// <summary>
        /// 起始端口
        /// </summary>
        public string FromPort;
    }
}
