using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FNode.Runtime
{
    public class RuntimeNodeData : INodeData
    {

        /// <summary>
        /// GUID
        /// </summary>
        public string GUID { get; set; }


        /// <summary>
        /// 唯一键
        /// </summary>
        public string UniqueName { get; set; }



        /// <summary>
        /// 后继
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, RuntimeNodeData> Successors { get; private set; } = new Dictionary<string, RuntimeNodeData>();


        /// <summary>
        /// 前驱
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, RuntimeNodeData> Predecessors { get; private set; } = new Dictionary<string, RuntimeNodeData>();


        /// <summary>
        /// 返回某一个后继节点
        /// </summary>
        [JsonIgnore]
        public RuntimeNodeData Next
        {
            get
            {
                foreach(var item in Successors)
                    return item.Value;
                return null;
            }
        }
        /// <summary>
        /// 返回某一个前驱节点
        /// </summary>
        [JsonIgnore]
        public RuntimeNodeData Last
        {
            get
            {
                foreach (var item in Predecessors)
                    return item.Value;
                return null;
            }
        }


    }
}
