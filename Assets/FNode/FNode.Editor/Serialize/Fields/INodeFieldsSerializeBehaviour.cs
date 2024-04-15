using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{
    /// <summary>
    /// 节点字段序列化行为
    /// </summary>
    public interface INodeFieldsSerializeBehaviour
    {
        public void InternalDeserialize(string json);
        public string InternalSerialize();
    }
}
