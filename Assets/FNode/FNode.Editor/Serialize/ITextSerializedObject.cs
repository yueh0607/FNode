using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{

    public interface ITextSerializedObject
    {

        void Deserialize(string str);

        string Serialize();
    }
}
