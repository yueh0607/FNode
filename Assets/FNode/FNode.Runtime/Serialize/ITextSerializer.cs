using System;
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


    /// <summary>
    /// 文本序列化接口: 能提供 将 T类型对象序列化为字符串 和 将字符串反序列化为T类型对象的能力
    /// </summary>
    public interface IUnknownTextSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string SerializeObject(object obj,Type type, int settingGroup = 0);


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        object DeserializeObject(string str,Type type, int settingGroup = 0);

        /// <summary>
        /// 把字符串填充到对象
        /// </summary>
        /// <param name="str"></param>
        /// <param name="obj"></param>
        /// <param name="settingGroup"></param>
        void MapToObject(string str, object obj, int settingGroup = 0);
    }
}
