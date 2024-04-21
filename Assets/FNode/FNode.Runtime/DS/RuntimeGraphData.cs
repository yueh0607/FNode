using FNode.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNode.Runtime
{
    public class RuntimeGraphData
    {
       
        /// <summary>
        /// 节点
        /// </summary>
        public List<RuntimeNodeData> Nodes { get; private set; } = new List<RuntimeNodeData>();

        /// <summary>
        /// 连接
        /// </summary>
        public List<RuntimeLinkData> Links { get; private set; } = new List<RuntimeLinkData>();

        [JsonIgnore]
        public List<RuntimeNodeData> Entries { get; protected set; } = new List<RuntimeNodeData>();

        [JsonIgnore]
        public Dictionary<string, RuntimeNodeData> NodeMapper { get; private set; } = new Dictionary<string, RuntimeNodeData>();


       
        [JsonConstructor]
        public RuntimeGraphData(List<RuntimeNodeData> nodes,List<RuntimeLinkData> links)
        {
            foreach(var node in nodes)
            {
                Nodes.Add(node);
                NodeMapper.Add(node.GUID, node);
            }
            foreach(var link in links)
            {
                Links.Add(link);
                NodeMapper[link.FromGUID].Successors.Add(link.ToGUID, NodeMapper[link.ToGUID]);
                NodeMapper[link.ToGUID].Predecessors.Add(link.FromGUID, NodeMapper[link.FromGUID]);
            }
            foreach(var node in nodes)
            {
                if(node.Successors.Count == 0)
                    Entries.Add(node);
            }
        }


    }
}
