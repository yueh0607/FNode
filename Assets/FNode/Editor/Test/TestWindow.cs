using Assets.FNode;
using System.Collections;
using System.Collections.Generic;
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

        view = new FGraphView(this, provider);
        this.rootVisualElement.Add(view);

        Toolbar toolbar = new Toolbar();
        toolbar.Add(new ToolbarButton() { text = "工具" });


        view.Add(toolbar);
        view.ImportFrom("Assets/MyGraph.asset");
    }

    private void OnDestroy()
    {
        if (view.Changed)
            view.ExportTo("Assets/MyGraph.asset");
    }
}
