using FNode.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Runtime
{
    public static class RuntimeGraphUtility 
    {

        /// <summary>
        /// 序列化运行时图数据
        /// </summary>
        /// <param name="grapgData"></param>
        /// <returns></returns>
        public static string SerializeRuntimeGraph(RuntimeGraphData grapgData)
        {
            return TextSerializeStrategy.Serialize<RuntimeGraphData>(grapgData, 2);
        }

        /// <summary>
        /// 反序列化运行时图数据
        /// </summary>
        /// <param name="grapgData"></param>
        /// <returns></returns>
        public static RuntimeGraphData DeserializeRuntimeGraph(string graphStr)
        {
            return TextSerializeStrategy.Deserialize<RuntimeGraphData>(graphStr, 2);
        }
    }
}
