using FNode.Runtime;
using System;

namespace FNode.Editor
{
    public abstract class GenericNodeBase<T> : NodeBase where T : RuntimeNodeData
    {
        private T fieldsInfo = null;

        /// <summary>
        /// 字段信息
        /// </summary>
        public T FieldsInfo
        {
            get
            {
                if (fieldsInfo == null)
                    fieldsInfo = Activator.CreateInstance<T>();
                return fieldsInfo;
            }
        }


        public GenericNodeBase(string nodeName) : base(nodeName)
        {

        }

        protected override string OnSerialize()
        {
            SyncToData();
            return TextSerializeStrategy.Serialize(FieldsInfo, 1);

        }
        protected override void OnDeserialize(string json)
        {
            fieldsInfo = TextSerializeStrategy.Deserialize<T>(json, 1);
            SyncFromData();
        }


        protected abstract void SyncToData();

        protected abstract void SyncFromData();
    }
}