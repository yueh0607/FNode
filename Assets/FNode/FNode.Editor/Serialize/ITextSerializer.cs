using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{
    /// <summary>
    /// 文本序列化接口
    /// </summary>
    public interface ITextSerializer 
    {
        string Serialize<T>(T obj);

        T Deserialize<T>(string str);
    }
}
