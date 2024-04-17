using FNode.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestNodeData
{
    public string textInfo;
}


[GraphViewMenuItem("TestNode","Test/TestNode",0,1)]
public class TestNode : GenericNodeBase<TestNodeData>
{
    TextField txt;
    public TestNode() : base("测试节点")
    {
        CreateInputPort("测试输入","TestInput", typeof(int), false);
        CreateOutputPort("测试输出","TestOutput", typeof(int), false);

        txt = new TextField("Txt");
        AddContent(txt);
    }

    protected override void SyncFromData()
    {
        txt.value = FieldsInfo.textInfo;
    }

    protected override void SyncToData()
    {
        FieldsInfo.textInfo = txt.value;
    }
}
