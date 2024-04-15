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
    }

    /// <summary>
    /// 搜索菜单提供者
    /// </summary>
    public class FSearchMenuWindowProvider : ScriptableObject, ISearchMenuWindowProvider
    {
        /// <summary>
        /// 搜索的节点拥有者ID
        /// </summary>
        public int OwnerID { get; set; } = 0;

        /// <summary>
        /// 菜单树列表
        /// </summary>
        public List<SearchTreeEntry> Entries = new List<SearchTreeEntry>();


        /// <summary>
        /// 映射字典（需要构建Mappper）
        /// </summary>
        public Dictionary<string, GraphViewMenuItemAttribute> UniqueMapper { get; private set; } = new Dictionary<string, GraphViewMenuItemAttribute>();
        /// <summary>
        /// 类-特性表(需要构建Mapper)
        /// </summary>
        public List<(Type, GraphViewMenuItemAttribute)> Menus = new List<(Type, GraphViewMenuItemAttribute)>();
        /// <summary>
        /// 唯一名-类型映射(需要构建Mapper)
        /// </summary>
        public Dictionary<string, Type> TypeMapper { get; set; } = new Dictionary<string, Type>();

        private class NodeTreeMenu
        {
            public string MenuName = string.Empty;
            public int Level = 1;
            public List<NodeTreeMenu> Children = new List<NodeTreeMenu>();
            public Type NodeType;
        }

        bool mapperBuilded = false;
        public bool MapppersBuilded => mapperBuilded;

        /// <summary>
        /// 构建映射信息
        /// </summary>
        public void BuildMappers()
        {
           

            Assembly assembly = Assembly.GetAssembly(typeof(GraphViewMenuItemAttribute));
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var atts = type.GetCustomAttributes<GraphViewMenuItemAttribute>();
                if (atts != null)
                    foreach (var att in atts)
                    {
                        if (!att.Owner.Contains(OwnerID)) continue;
                        Menus.Add((type, att));
                        TypeMapper.TryAdd(att.UniqueKey, type);
                    }
            }
            Menus.Sort((x, y) => y.Item2.Parts.Length - x.Item2.Parts.Length);
            //建立映射字典
            foreach (var tuple in Menus)
            {
                UniqueMapper.TryAdd(tuple.Item2.UniqueKey, tuple.Item2);
            }

            mapperBuilded = true;
        }


        bool menuTreeBuilded = false;
        public bool MenuTreeBuilded => menuTreeBuilded;
        /// <summary>
        /// 构建菜单树
        /// </summary>
        public void BuildMenuTree()
        {
            
            Entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

            NodeTreeMenu menu = new NodeTreeMenu();

            //最大层数
            int maxLayer = Menus[0].Item2.Parts.Length;
            //构造菜单树
            for (int i = 0; i < maxLayer; i++)
            {
                foreach (var tuple in Menus)
                {
                    //路径层数-当前走过的层数，得到剩余层数
                    int value = tuple.Item2.Parts.Length - i;
                    //剩余层数小于1，代表此菜单项已经走到尽头
                    if (value < 1) continue;

                    //当前项的名称需要访问（当前层数 + 剩余层数 -1）位置的值
                    string curItem = tuple.Item2.Parts[i + value - 1];
                    menu.Children.Add(new NodeTreeMenu() { MenuName = curItem, Level = i + 1, NodeType = tuple.Item1 });
                }
            }

            //中序遍历展开树
            Stack<NodeTreeMenu> stack = new Stack<NodeTreeMenu>();
            menu.Children.ForEach((menu) =>
            {
                stack.Push(menu);
                while (stack.Count != 0)
                {
                    var node = stack.Pop();

                    if (node.Children.Count != 0)
                        Entries.Add(new SearchTreeGroupEntry(new GUIContent(node.MenuName)) { level = node.Level });
                    else
                        Entries.Add(new SearchTreeEntry(new GUIContent(node.MenuName)) { level = node.Level, userData = node.NodeType });
                    //子节点入栈
                    foreach (var n in node.Children)
                        stack.Push(n);
                }
            });

            menuTreeBuilded = true;
        }



        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (!MapppersBuilded) BuildMappers();
            if(!MenuTreeBuilded) BuildMenuTree();

            return Entries;
        }

        /// <summary>
        /// 在选择一个菜单项时被调用
        /// </summary>
        public SerchMenuWindowOnSelectEntryDelegate OnSelectEntryHandler { get; set; }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (OnSelectEntryHandler == null)
                return false;
            return OnSelectEntryHandler(searchTreeEntry, context);
        }

    }
}
