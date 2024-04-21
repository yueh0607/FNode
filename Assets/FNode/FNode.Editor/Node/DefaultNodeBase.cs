using FNode.Editor;
using FNode.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmptyNodeData : RuntimeNodeData
{

}
public abstract class DefaultNodeBase : GenericNodeBase<EmptyNodeData>
{
    public DefaultNodeBase(string nodeName) : base(nodeName)
    {

    }
    protected override void SyncToData()
    {

    }

    protected override void SyncFromData()
    {

    }

}
