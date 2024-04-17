using FNode.Editor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class TestWindow : EditorWindow
{
    [MenuItem("Test", menuItem = "Test/Grpah")]
    static void Open()
    {
        var window = GetWindow<TestWindow>();
        window.Show();
        window.Init();

    }
    FGraphView view;
    void Init()
    {

        var provider = ScriptableObject.CreateInstance<FSearchMenuWindowProvider>();
        provider.OwnerID = 0;
       
        view = new FGraphView(this, provider);
        this.rootVisualElement.Add(view);

        Toolbar toolbar = new Toolbar();
        toolbar.Add(new ToolbarButton() { text = "工具" });


        view.Add(toolbar);
        if (!File.Exists("Assets/MyGraph.json"))
        {
            File.Create("Assets/MyGraph.json").Dispose();
        }
        view.Deserialize(File.ReadAllText("Assets/MyGraph.json"));
        AssetDatabase.Refresh();
    }

    private void OnDestroy()
    {
        if (view.Changed)
        {
            if (!File.Exists("Assets/MyGraph.json"))
            {
                File.Create("Assets/MyGraph.json").Dispose();
            }
            File.WriteAllText("Assets/MyGraph.json", view.Serialize());
        }
        AssetDatabase.Refresh();
    }
}
