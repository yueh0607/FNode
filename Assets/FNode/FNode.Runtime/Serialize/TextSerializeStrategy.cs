using System;

namespace FNode.Editor
{
    internal enum SerializeType
    {
        NewtonsoftJson
    }
    public static class TextSerializeStrategy
    {
        const SerializeType type = SerializeType.NewtonsoftJson;

        private static object GetSerialize()
        {
            switch (type)
            {
                case SerializeType.NewtonsoftJson:
                    return JsonTextSerializer.Instance;
            }
            throw new System.Exception("Not supported");
        }


        public static string Serialize<T>(T obj, int settingGroup=0)
        {
            var serializer = GetSerialize() as ITextSerializer;
            return serializer.Serialize(obj, settingGroup);
        }

        public static T Deserialize<T>(string text,int settingGroup=0)
        {
            var serializer = GetSerialize() as ITextSerializer;
            return serializer.Deserialize<T>(text, settingGroup);
            
        }

        public static object DeserializeObject(string text,Type type, int settingGroup = 0)
        {
            var serializer = GetSerialize() as IUnknownTextSerializer;
            return serializer.DeserializeObject(text,type, settingGroup);
        }

        public static string SerializeObject(object obj,Type type, int settingGroup = 0)
        {
            var serializer = GetSerialize() as IUnknownTextSerializer;
            return serializer.SerializeObject(obj,type , settingGroup);
        }

        public static void MapToObject(string text, object obj, int settingGroup = 0)
        {
            var serializer = GetSerialize() as IUnknownTextSerializer;
            serializer.MapToObject(text, obj, settingGroup);
        }
    }
}
