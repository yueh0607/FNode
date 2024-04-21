using FNode.Editor;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TestWindow : EditorWindow
{
    private const string GraphFilePath = "Assets/MyGraph.json";
    private const string EditorPrefsKey = "MyGraph";

    private FGraphView view;

    [MenuItem("Test/Graph")]
    public static void Open()
    {
        var window = GetWindow<TestWindow>();
        window.Show();
        window.Initialize();
    }

    private void Initialize()
    {
        view = CreateGraphView();
        SetupToolbar();

        LoadGraphFromFile();
        LoadViewData();
    }

    private FGraphView CreateGraphView()
    {
        var provider = ScriptableObject.CreateInstance<FSearchMenuWindowProvider>();
        provider.OwnerID = 0;

        var graphView = new FGraphView(this, provider);
        rootVisualElement.Add(graphView);

        return graphView;
    }

    private void SetupToolbar()
    {
        var toolbar = new Toolbar();
        var toolbarButton = new ToolbarButton { text = "工具" };
        toolbar.Add(toolbarButton);
        Button exportBtn = new Button(() =>
        {
            EditorGraph2RuntimeGraph.ExportToRuntimeGraph(GraphFilePath, GraphFilePath.Replace("MyGraph.json", "MyGraph_exported.json"));
        }) 
        {
            text = "导出"
        };
        toolbar.Add(exportBtn);

        view.Add(toolbar);
    }

    private void LoadGraphFromFile()
    {
        if (!File.Exists(GraphFilePath))
        {
            File.Create(GraphFilePath).Dispose();
            EditorPrefs.DeleteKey(EditorPrefsKey);
        }

        string graphData = File.ReadAllText(GraphFilePath);
        view.Deserialize(graphData);
    }

    private void LoadViewData()
    {
        string viewData = EditorPrefs.GetString(EditorPrefsKey);
        view.SetViewData(viewData);
    }

    private void OnDestroy()
    {
        SaveGraphToFile();
        SaveViewData();
        AssetDatabase.Refresh();
    }

    private void SaveGraphToFile()
    {
        if (view.Changed)
        {
            string graphData = view.Serialize();
            File.WriteAllText(GraphFilePath, graphData);
        }
    }

    private void SaveViewData()
    {
        string viewData = view.GetViewData();
        EditorPrefs.SetString(EditorPrefsKey, viewData);
    }
}
