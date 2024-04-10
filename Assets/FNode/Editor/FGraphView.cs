using Assets.FNode;
using Assets.FNode.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class FGraphView : GraphView
{
    EditorWindow _editorWindow;
    FSearchMenuWindowProvider _provider;
    public event Action<NodeBase> OnNodeCreate = null;
    public event Action<NodeBase> OnNodeRemove = null;

    public FGraphView(EditorWindow editorWindow, FSearchMenuWindowProvider provider)
    {
        _provider = provider;
        _editorWindow = editorWindow;
        provider.OnSelectEntryHandler = (searchTreeEntry, searchWindowContext) =>
        {
            var windowRoot = _editorWindow.rootVisualElement;
            var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent, searchWindowContext.screenMousePosition - _editorWindow.position.position);
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



        var styleSheet = EditorGUIUtility.Load("NodeGraphView.uss") as StyleSheet;
        styleSheets.Add(styleSheet);
        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();


        this.graphViewChanged += (change) =>
        {
            Changed = true;
            return change;
        };
    }


    /// <summary>
    /// 是否已经改变
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

    /// <summary>
    /// 导出指定的Asset目录
    /// </summary>
    /// <param name="assetPath"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void ExportTo(string assetPath)
    {
        if (!Path.HasExtension(assetPath)) throw new InvalidOperationException("assetPath must be a file");
        DirectoryInfo dataPath = new DirectoryInfo(Application.dataPath);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(dataPath.Parent.FullName, assetPath)));

        if (File.Exists(Path.Combine(dataPath.Parent.FullName, assetPath)))
        {

            GraphData data = AssetDatabase.LoadAssetAtPath<GraphData>(assetPath);
            Export(data);
        }
        else
        {
            GraphData data = ScriptableObject.CreateInstance<GraphData>();
            AssetDatabase.CreateAsset(data, assetPath);
            Export(data);

        }
    }
    public void Export(GraphData data)
    {
        GraphData tempData = ScriptableObject.CreateInstance<GraphData>();
        bool result = true;
        try
        {
            //保存节点信息
            foreach (var node in nodes)
            {
                NodeBase nodeBase = (NodeBase)node;

                NodeData nodeData = new NodeData();
                nodeData.GUID = nodeBase.GUID;
                nodeData.UniqueName = nodeBase.GetType().GetCustomAttribute<GraphViewMenuItemAttribute>().UniqueKey;
                nodeData.Position = nodeBase.Position;
                nodeData.Fields = nodeBase.SerializeJson();

                tempData.nodes.Add(nodeData);
            }
            //保存边信息
            foreach (var edge in edges)
            {
                LinkData linkData = new LinkData();
                linkData.FromGUID = ((NodeBase)(edge.output.node)).GUID;
                linkData.ToGUID = ((NodeBase)(edge.input.node)).GUID;
                linkData.FromPort = edge.output.name;
                linkData.ToPort = edge.input.name;
                tempData.links.Add(linkData);
            }

        }
        catch (Exception e)
        {
            result = false;
            throw new InvalidOperationException($"Failed to save.\n\nDetails:\n{e}");
        }
        if (result)
        {
            data.links.Clear();
            data.nodes.Clear();
            foreach (var node in tempData.nodes)
            {
                data.nodes.Add(node);
            }
            foreach (var link in tempData.links)
            {
                data.links.Add(link);
            }


            data.view.Position = viewTransform.position;
            data.view.Scale = viewTransform.scale;
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    public void ImportFrom(string assetPath)
    {
        if (!Path.HasExtension(assetPath)) throw new InvalidOperationException("assetPath must be a file");
        DirectoryInfo dataPath = new DirectoryInfo(Application.dataPath);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(dataPath.Parent.FullName, assetPath)));

        if (File.Exists(Path.Combine(dataPath.Parent.FullName, assetPath)))
        {
            GraphData data = AssetDatabase.LoadAssetAtPath<GraphData>(assetPath);
            Import(data);
        }
        else
        {
            GraphData data = ScriptableObject.CreateInstance<GraphData>();
            AssetDatabase.CreateAsset(data, assetPath);
            Import(data);
        }


    }
    public void Import(GraphData data)
    {
        if (!_provider.MapppersBuilded) _provider.BuildMappers();
        Dictionary<string, NodeBase> nodeMapper = new Dictionary<string, NodeBase>();

        //恢复节点信息
        foreach (var nodeData in data.nodes)
        {
            if (_provider.TypeMapper.TryGetValue(nodeData.UniqueName, out Type type))
            {
                if (_provider.UniqueMapper.TryGetValue(nodeData.UniqueName, out var attribute))
                {
                    var node = CreateNode(type, nodeData.Position);
                    node.DeserializeJson(nodeData.Fields);
                    node.GUID = nodeData.GUID;
                    nodeMapper.TryAdd(node.GUID, node);
                }
            }
            else Debug.LogError($"丢失的节点类：{nodeData.UniqueName} 原信息将会在下一次保存时永久丢失");
        }
        //恢复连接信息
        foreach (var linkData in data.links)
        {
            if (nodeMapper.TryGetValue(linkData.FromGUID, out var nodeFrom))
            {
                if (nodeMapper.TryGetValue(linkData.ToGUID, out var nodeTo))
                {
                    var from = nodeFrom.TryGetOutputPort(linkData.FromPort);
                    var to = nodeTo.TryGetInputPort(linkData.ToPort);

                    if (from != null && to != null)
                    {
                        Connect(from, to);
                    }
                }
                else Debug.LogError($"丢失的节点：{linkData.ToGUID} 原信息将会在下一次保存时永久丢失");
            }
            else Debug.LogError($"丢失的节点：{linkData.FromGUID} 原信息将会在下一次保存时永久丢失");
        }

        //恢复观察信息
        viewTransform.position = data.view.Position;
        viewTransform.scale = data.view.Scale;
    }

}

