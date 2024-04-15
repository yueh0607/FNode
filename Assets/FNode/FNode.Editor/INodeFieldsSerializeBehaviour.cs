using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{
    public interface INodeFieldsSerializeBehaviour
    {
        public void InternalDeserialize(string json);
        public string InternalSerialize();
    }
}
