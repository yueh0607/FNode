using Assets.FNode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[GraphViewMenuItem("TestNode","Test/TestNode",0,1)]
public class TestNode : DefaultNodeBase
{
    public TestNode() : base("测试节点")
    {
        CreateInputPort("测试输入","TestInput", typeof(int), false);
        CreateOutputPort("测试输出","TestOutput", typeof(int), false);

        TextField txt = new TextField();
        AddContent(txt);
        

    }
}
