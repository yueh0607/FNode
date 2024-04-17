using Codice.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace FNode.Editor
{
    public class FGraphView : GraphView, ITextSerializedObject
    {
        EditorWindow _editorWindow;
        FSearchMenuWindowProvider _provider;

        public event Action<NodeBase> OnNodeCreate = null;
        public event Action<NodeBase> OnNodeRemove = null;

        public FGraphView(EditorWindow editorWindow, FSearchMenuWindowProvider provider)
        {
            _provider = provider;
            _editorWindow = editorWindow;

            //在右键菜单中添加节点
            provider.OnSelectEntryHandler = (searchTreeEntry, searchWindowContext) =>
            {
                var windowRoot = _editorWindow.rootVisualElement;
                var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot, searchWindowContext.screenMousePosition - _editorWindow.position.position);
                var graphMousePosition = contentViewContainer.WorldToLocal(windowMousePosition);
                var type = searchTreeEntry.userData as Type;
                CreateNode(type, graphMousePosition);
                return true;
            };
            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), provider);
            };

            //尺寸和父控件相同
            this.StretchToParentSize();
            //滚轮缩放
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            //窗口内容拖动
            this.AddManipulator(new ContentDragger());
            //选中Node移动功能
            this.AddManipulator(new SelectionDragger());
            //多个node框选功能
            this.AddManipulator(new RectangleSelector());


            //加载样式表和网格
            var styleSheet = EditorGUIUtility.Load("NodeGraphView.uss") as StyleSheet;
            styleSheets.Add(styleSheet);
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            //当图变化时，设置为已改变
            this.graphViewChanged += (change) =>
            {
                Changed = true;
                return change;
            };
        }


        /// <summary>
        /// 是否已经改变 : 仅包含  节点的新增/删除/移动/连接/断开，注意不包含节点字段的修改
        /// </summary>
        public bool Changed { get; private set; } = false;

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        public NodeBase CreateNode(Type type, Vector2 position)
        {
            //创建节点实例
            NodeBase node = Activator.CreateInstance(type) as NodeBase;
            node.Position = position;
            this.AddElement(node);

            return node;
        }

        /// <summary>
        /// 连接端口
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public Edge Connect(Port from, Port to)
        {

            var edge = from.ConnectTo(to);
            this.Add(edge);
            return edge;
        }

        /// <summary>
        /// 定义可连接的端口规则
        /// </summary>
        /// <param name="startAnchor"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            //端口类型相同、方向不同、节点不同
            var compatiblePorts = new List<Port>();
            foreach (var port in ports.ToList())
            {
                if (startAnchor.node == port.node ||
                    startAnchor.direction == port.direction ||
                    startAnchor.portType != port.portType)
                {
                    continue;
                }

                compatiblePorts.Add(port);
            }
            return compatiblePorts;
        }


        public void Deserialize(string str)
        {
            GraphData data =  TextSerializeStrategy.Deserialize<GraphData>(str,1);

            Dictionary<string, NodeBase> nodeMapper = new Dictionary<string, NodeBase>();

            if (_provider.UniqueMapper == null) 
                _provider.OnBuildUniqueMapperAndMenuTree();

            //恢复节点信息
            foreach (var nodeData in data.nodes)
            {
                if (_provider.UniqueMapper.TryGetValue(nodeData.UniqueName, out var item))
                {
                    var node = CreateNode(item.AssociationType, nodeData.Position);
                    ((INodeFieldsSerializeBehaviour)node).InternalDeserialize(nodeData.Fields);
                    node.GUID = nodeData.GUID;
                    nodeMapper.TryAdd(node.GUID, node);
                }
                else Debug.LogError($"丢失的节点类：{nodeData.UniqueName} 此节点将会在下一次保存时永久丢失");
            }
            //恢复连接信息
            foreach (var linkData in data.links)
            {
                if (nodeMapper.TryGetValue(linkData.FromGUID, out var nodeFrom))
                {
                    if (nodeMapper.TryGetValue(linkData.ToGUID, out var nodeTo))
                    {
                        var fromResult = nodeFrom.TryGetOutputPort(linkData.FromPort, out var from);
                        var toResult = nodeTo.TryGetInputPort(linkData.ToPort, out var to);

                        if (fromResult && toResult)
                        {
                            Connect(from, to);
                        }
                    }
                    else Debug.LogError($"丢失的节点：{linkData.ToGUID} 此节点及其相关的连接将会在下一次保存时永久丢失");
                }
                else Debug.LogError($"丢失的节点：{linkData.FromGUID} 此节点及其相关的连接将会在下一次保存时永久丢失");
            }

            //恢复观察信息
            viewTransform.position = data.view.Position;
            viewTransform.scale = data.view.Scale;
        }

        public string Serialize()
        {
            GraphData data = Activator.CreateInstance<GraphData>();
            foreach (var node in nodes)
            {
                NodeBase nodeBase = (NodeBase)node;

                NodeData nodeData = new NodeData();
                nodeData.GUID = nodeBase.GUID;
                nodeData.UniqueName = nodeBase.GetType().GetCustomAttribute<GraphViewMenuItemAttribute>().UniqueKey;
                nodeData.Position = nodeBase.Position;
                nodeData.Fields = ((INodeFieldsSerializeBehaviour)nodeBase).InternalSerialize();
                data.nodes.Add(nodeData);
            }
            //保存边信息
            foreach (var edge in edges)
            {
                LinkData linkData = new LinkData();
                linkData.FromGUID = ((NodeBase)(edge.output.node)).GUID;
                linkData.ToGUID = ((NodeBase)(edge.input.node)).GUID;
                linkData.FromPort = edge.output.name;
                linkData.ToPort = edge.input.name;
                data.links.Add(linkData);
            }
            string result = TextSerializeStrategy.Serialize(data,1);
            return result;
        }
    }

}