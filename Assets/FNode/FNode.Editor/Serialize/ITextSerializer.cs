using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{
    /// <summary>
    /// 文本序列化接口: 能提供 将 T类型对象序列化为字符串 和 将字符串反序列化为T类型对象的能力
    /// </summary>
    public interface ITextSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string Serialize<T>(T obj, int settingGroup = 0);


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        T Deserialize<T>(string str, int settingGroup = 0);
    }
}
