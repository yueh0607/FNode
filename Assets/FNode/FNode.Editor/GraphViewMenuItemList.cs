using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace FNode.Editor
{
    public static class GraphViewMenuItemList
    {
        private static GraphViewMenuItem[] _graphViewMenuItems;
        private static string _path = Path.Combine(Application.dataPath, "FNodeSetting", "GraphViewMenuItems.json");


        [MenuItem("Test/BuildGraphViewMenu")]
        /// <summary>
        /// 构建GraphView菜单项列表的缓存文件
        /// 注意：该方法会覆盖配置文件
        /// </summary>
        public static void BuildMenu()
        {
            List<GraphViewMenuItem> MenuItemList = new List<GraphViewMenuItem>(256);

            //从所有程序集中检索
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                //动态程序集不能获取类型
                if (assembly.IsDynamic) continue;

                Type[] types = assembly.GetExportedTypes();

                foreach (Type type in types.ToArray())
                {
                    var attribute = type.GetCustomAttribute<GraphViewMenuItemAttribute>();

                    if (attribute is not null)
                    {
                        MenuItemList.Add(new GraphViewMenuItem(attribute, type));
                    }
                }
            }

            _graphViewMenuItems = MenuItemList.ToArray();
            //保存到文件
            if (!File.Exists(_path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_path));
                File.Create(_path);
            }

            string json = JsonConvert.SerializeObject(_graphViewMenuItems, Formatting.Indented);
            File.WriteAllText(_path, json);
        }

        [MenuItem("Test/GetGraphViewMenuItems")]
        /// <summary>
        /// 获取GraphView菜单项列表
        /// 注意：在编辑器中调用该方法，需要保证配置文件存在
        /// 否则会返回null
        /// </summary>
        /// <returns></returns>
        public static GraphViewMenuItem[] GetGraphViewMenuItems()
        {
            //直接返回
            if (_graphViewMenuItems is not null)
            {
                // foreach (var item in _graphViewMenuItems)
                // {
                //     Debug.Log(item);
                // }
                return _graphViewMenuItems;
            }

            //从配置文件获取
            if (File.Exists(_path))
            {
                string json = File.ReadAllText(_path);
                _graphViewMenuItems = JsonConvert.DeserializeObject<GraphViewMenuItem[]>(json);
                // foreach (var item in _graphViewMenuItems)
                // {
                //     Debug.Log(item);
                // }
                return _graphViewMenuItems;
            }

            //异常
            Debug.LogError("GraphViewMenuItems.json not found");
            return null;
        }

        // [MenuItem("Test/ClearGraphViewMenuItems")]
        // public static void Clear()
        // {
        //     _graphViewMenuItems = null;
        // }
    }
}
