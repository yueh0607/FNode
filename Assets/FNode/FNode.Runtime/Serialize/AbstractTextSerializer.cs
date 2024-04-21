using System;
using System.Threading;

namespace FNode.Editor
{

    public abstract class AbstractTextSerializer<T> : ITextSerializer,IUnknownTextSerializer where T : AbstractTextSerializer<T>
    {
        private static volatile T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = Activator.CreateInstance<T>();
                return instance;
            }
        }

        public abstract DataType Deserialize<DataType>(string str, int settingGroup = 0);
        public abstract object DeserializeObject(string str ,Type type , int settingGroup = 0);

        public abstract void MapToObject(string str, object obj, int settingGroup = 0);

        public abstract string Serialize<DataType>(DataType obj, int settingGroup = 0);
        public abstract string SerializeObject(object obj ,Type type, int settingGroup = 0);
    }
}
