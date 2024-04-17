using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace FNode.Editor
{
    public delegate bool SerchMenuWindowOnSelectEntryDelegate(SearchTreeEntry searchTreeEntry,
    SearchWindowContext context);
    public interface ISearchMenuWindowProvider : ISearchWindowProvider
    {
        /// <summary>
        /// 归属者ID : 指明当前图能找到的节点ID
        /// </summary>
        int OwnerID { get; }

        /// <summary>
        /// 在进入右键菜单时用于处理的委托
        /// </summary>
        SerchMenuWindowOnSelectEntryDelegate OnSelectEntryHandler { get; set; }

        /// <summary>
        /// 构建的节点信息缓存字典 ，如果为空则需要Build
        /// </summary>
        Dictionary<string, GraphViewMenuItemAttribute> UniqueMapper { get; }

        /// <summary>
        /// 构建节点信息缓存
        /// </summary>
        void OnBuildUniqueMapperAndMenuTree();
    }

    /// <summary>
    /// 搜索菜单提供者
    /// </summary>
    public class FSearchMenuWindowProvider : ScriptableObject, ISearchMenuWindowProvider
    {
        public int OwnerID { get; set; } = 0;
        public List<SearchTreeEntry> Entries = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Create Node"))
        };

        public Dictionary<string, GraphViewMenuItemAttribute> UniqueMapper { get; private set; } = null;
        private List<GraphViewMenuItemAttribute> Menus = null;


        private class NodeTreeMenu
        {
            public string MenuName = string.Empty;
            public int Level = 1;
            public List<NodeTreeMenu> Children = new List<NodeTreeMenu>();
            public Type NodeType;
        }

        public void OnBuildUniqueMapperAndMenuTree()
        {
            UniqueMapper = new Dictionary<string, GraphViewMenuItemAttribute>();
            Menus = new List<GraphViewMenuItemAttribute>();

            //读取节点描述信息
            GraphViewMenuItemAttribute[] nodeDescriptionItems = NodeCacheUtility.GetGraphViewMenuItems();


            foreach (GraphViewMenuItemAttribute item in nodeDescriptionItems)
            {
                if (!item.Owners.Contains(OwnerID)) continue;
                if (!UniqueMapper.TryAdd(item.UniqueKey, item))
                    Debug.LogError($"There is same key existed. key={item.UniqueKey}");
                else
                    Menus.Add(item);
            }
            //按路径长度降序排序
            Menus.Sort((x, y) => y.Parts.Length - x.Parts.Length);


            NodeTreeMenu menuNode = new NodeTreeMenu();

            //最大的路径层数
            int maxLayer = Menus[0].Parts.Length;
            //构造菜单树
            for (int i = 0; i < maxLayer; i++)
            {
                foreach (var nodeDescriptionItem in Menus)
                {
                    //路径层数-当前走过的层数，得到剩余层数
                    int remainLayerCount = nodeDescriptionItem.Parts.Length - i;

                    //剩余层数小于1，代表此菜单项已经走到尽头
                    if (remainLayerCount < 1) continue;

                    //当前项的名称需要访问（当前层数 + 剩余层数 -1）位置的值
                    string curItem = nodeDescriptionItem.Parts[i + remainLayerCount - 1];

                    menuNode.Children.Add(new NodeTreeMenu() { MenuName = curItem, Level = i +1 , NodeType = nodeDescriptionItem.AssociationType });
                }
            }

            //中序遍历展开树
            Stack<NodeTreeMenu> foreachStack = new Stack<NodeTreeMenu>();
            menuNode.Children.ForEach((menu) =>
            {
                foreachStack.Push(menu);
                while (foreachStack.Count != 0)
                {
                    var node = foreachStack.Pop();

                    //非叶子节点使用Group
                    if (node.Children.Count != 0)
                        Entries.Add(new SearchTreeGroupEntry(new GUIContent(node.MenuName)) { level = node.Level });
                    //叶子节点使用非Group
                    else
                        Entries.Add(new SearchTreeEntry(new GUIContent(node.MenuName)) { level = node.Level, userData = node.NodeType });
                    //子节点入栈，继续遍历
                    foreach (var n in node.Children)
                        foreachStack.Push(n);
                }
            });
        }


        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (UniqueMapper == null || Menus == null)
                OnBuildUniqueMapperAndMenuTree();
            return Entries;
        }

        public SerchMenuWindowOnSelectEntryDelegate OnSelectEntryHandler { get; set; }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (OnSelectEntryHandler == null)
                return false;
            return OnSelectEntryHandler(searchTreeEntry, context);
        }


        public static FSearchMenuWindowProvider Create(int owner = 0)
        {
            var provider = ScriptableObject.CreateInstance<FSearchMenuWindowProvider>();
            provider.OwnerID = owner;
            return provider;
        }
    }
}
