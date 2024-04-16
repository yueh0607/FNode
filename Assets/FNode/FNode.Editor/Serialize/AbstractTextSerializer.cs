using System;
using System.Threading;

namespace FNode.Editor
{

    public abstract class AbstractTextSerializer<T> : ITextSerializer where T : AbstractTextSerializer<T>
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

        public abstract DataType Deserialize<DataType>(string str);
        public abstract string Serialize<DataType>(DataType obj);
    }
}
