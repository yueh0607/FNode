using FNode.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmptyNodeData
{

}
public abstract class DefaultNodeBase : GenericNodeBase<EmptyNodeData>
{
    public DefaultNodeBase(string nodeName) : base(nodeName)
    {

    }
}
