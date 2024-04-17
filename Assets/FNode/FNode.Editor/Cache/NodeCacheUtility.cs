using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FNode.Editor
{
    public static class NodeCacheUtility
    {
        private static GraphViewMenuItemAttribute[] nodeDescriptionItems = null;

        private static readonly string cachePath = Path.Combine(Application.dataPath, "FNodeSetting", "GraphViewMenuItems.json");


        /// <summary>
        /// 构建节点缓存
        /// </summary>
        [MenuItem("FNode/BuildNodeCache")]
        public static void BuildNodeCache()
        {
            Stopwatch watch = Stopwatch.StartNew();
            List<GraphViewMenuItemAttribute> nodeDescriptionItemList = new List<GraphViewMenuItemAttribute>(256);

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {

                if (assembly.IsDynamic) continue;

                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    var attribute = type.GetCustomAttribute<GraphViewMenuItemAttribute>();
                    if (attribute != null)
                        nodeDescriptionItemList.Add(new GraphViewMenuItemAttribute(attribute, type));
                }
            }

            nodeDescriptionItems = nodeDescriptionItemList.ToArray();


            if (!File.Exists(cachePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(cachePath));
                File.Create(cachePath).Dispose();
            }

            string text = string.Empty;
            try
            {
                text = JsonTextSerializer.Instance.Serialize(nodeDescriptionItems);
            }
            catch (Exception e)
            {
                throw new FormatException($"GraphViewMenuItems.json Serialize Error {System.Environment.NewLine} {e}");
            }

            //如果写占用
            if (FileAccessUtility.IsFileWriteInUse(cachePath))
                throw new IOException("GraphViewMenuItems.json is in use, Please close it and try again");

            File.WriteAllText(cachePath, text);
            watch.Stop();
            UnityEngine.Debug.Log($"Build Node Cache Success in {watch.Elapsed.TotalSeconds}s , Total {nodeDescriptionItemList.Count}");


        }

        /// <summary>
        /// 获取节点缓存
        /// </summary>
        /// <returns></returns>
        public static GraphViewMenuItemAttribute[] GetGraphViewMenuItems()
        {
            if (nodeDescriptionItems == null)
            {
                //从配置文件获取
                if (File.Exists(cachePath))
                {
                    //如果读占用
                    if (FileAccessUtility.IsFileReadInUse(cachePath))
                        throw new IOException("GraphViewMenuItems.json is in use, Please close it and try again");
                    string text = File.ReadAllText(cachePath);

                    //反序列化异常
                    try
                    {
                        nodeDescriptionItems = JsonTextSerializer.Instance.Deserialize<GraphViewMenuItemAttribute[]>(text);
                    }
                    catch (Exception e)
                    {
                        if (EditorUtility.DisplayDialog("Error", $"The cache file may be damaged, it is recommended to regenerate it, but we will leave the choice to you. If you click ignore, the cache will not be regenerated.", "Ignore", "Delete Cache And ReGenerate"))
                        {
                            throw new FormatException($"GraphViewMenuItems.json Deserialize Error {System.Environment.NewLine} {e}");
                        }
                        else
                        {
                            //删除文件重新生成
                            File.Delete(cachePath);
                            BuildNodeCache();
                        }
                    }
                }
                else
                {
                    if (EditorUtility.DisplayDialog("Error", $"The cache file does not exist. Click Generate Now to continue. You can also choose not to continue generating and use other methods to complete the cache file.", "Generate Now!", "Ignore"))
                    {
                        BuildNodeCache();
                        GetGraphViewMenuItems();
                    }
                    else throw new FileNotFoundException("GraphViewMenuItems.json not found, Please BuildNodeCaches at FNode/BuildNodeCache");
                }
            }
            return nodeDescriptionItems;
        }


    }
}
