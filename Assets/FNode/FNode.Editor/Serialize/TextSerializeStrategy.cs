using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{
    internal enum SerializeType
    {
        NewtonsoftJson
    }
    internal static class TextSerializeStrategy
    {
        const SerializeType type = SerializeType.NewtonsoftJson;
        public static string Serialize<T>(T obj, int settingGroup=0)
        {
            switch (type)
            {
                case SerializeType.NewtonsoftJson:
                    return JsonTextSerializer.Instance.Serialize<T>(obj,settingGroup);
            }
            throw new System.Exception("Not supported");
        }

        public static T Deserialize<T>(string text,int settingGroup=0)
        {
            switch (type)
            {
                case SerializeType.NewtonsoftJson:
                    return JsonTextSerializer.Instance.Deserialize<T>(text,settingGroup);
            }
            throw new System.Exception("Not supported");
        }
       
    }
}