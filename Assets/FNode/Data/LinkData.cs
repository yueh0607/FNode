using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FNode.Data
{

    /// <summary>
    /// 连接数据
    /// </summary>
    [Serializable]
    public class LinkData
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
