using FNode.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FNode.Editor
{
    public static class EditorGraph2RuntimeGraph
    {

        /// <summary>
        /// 将编辑器图数据转换为运行时图数据
        /// </summary>
        /// <param name="grpahData"></param>
        /// <returns></returns>
        public static RuntimeGraphData ToRuntimeGrpah(GraphData grpahData)
        {
            static Type GetGenericType(Type type)
            {
                if (type == null)
                    return null;
                // 获取类型的直接父类
                Type baseType = type.BaseType;
                // 如果父类是泛型类型并且是 GenericNode<T> 类型，则返回该泛型类型
                if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(GenericNodeBase<>))
                    return baseType;
                else return GetGenericType(baseType);
            }

            return new RuntimeGraphData
            (

                nodes: grpahData.nodes.ConvertAll((node) =>
                {
                    var nodeDataType = NodeCacheUtility
                    .GetGraphViewMenuItems()
                    .First((item) => item.UniqueKey == node.UniqueName)
                    .AssociationType;
                    nodeDataType = GetGenericType(nodeDataType).GetGenericArguments()[0];

                    //字段恢复成节点数据
                    RuntimeNodeData data = (RuntimeNodeData)TextSerializeStrategy.DeserializeObject(node.Fields, nodeDataType, 1);
                    data.GUID = node.GUID;
                    data.UniqueName = node.UniqueName;
                    return data;
                }),


                links: grpahData.links.ConvertAll(link => new RuntimeLinkData
                {
                    FromGUID = link.FromGUID,
                    ToGUID = link.ToGUID,
                    ToPort = link.ToPort,
                    FromPort = link.FromPort
                })
            );
        }



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

        /// <summary>
        /// 序列化运行时图数据
        /// </summary>
        /// <param name="grapgData"></param>
        /// <returns></returns>
        public static string SerializeEditorGraph(GraphData grapgData)
        {
            return TextSerializeStrategy.Serialize<GraphData>(grapgData, 1);
        }
        /// <summary>
        /// 反序列化运行时图数据
        /// </summary>
        /// <param name="grapgData"></param>
        /// <returns></returns>
        public static GraphData DeserializeEditorGraph(string graphStr)
        {
            return TextSerializeStrategy.Deserialize<GraphData>(graphStr, 1);
        }

        /// <summary>
        /// 导出编辑器图数据到运行时图数据
        /// </summary>
        /// <param name="fromProjectPath">必须存在的路径</param>
        /// <param name="toProjectPath">自动创建或覆盖的路径</param>
        public static void ExportToRuntimeGraph(string fromProjectPath, string toProjectPath)
        {
            FileAccessUtility.IfNotExistThrow(Path.Combine(FileAccessUtility.ProjectPath, fromProjectPath));
            var graphData = File.ReadAllText(Path.Combine(FileAccessUtility.ProjectPath, fromProjectPath));
            var editprGraphData = DeserializeEditorGraph(graphData);
            var runtimeGraphStr = SerializeRuntimeGraph(ToRuntimeGrpah(editprGraphData));
            FileAccessUtility.FileAndDirectoryMustExist(Path.Combine(FileAccessUtility.ProjectPath, toProjectPath));
            System.IO.File.WriteAllText(Path.Combine(FileAccessUtility.ProjectPath, toProjectPath), runtimeGraphStr);
            AssetDatabase.Refresh();
        }

    }
}
