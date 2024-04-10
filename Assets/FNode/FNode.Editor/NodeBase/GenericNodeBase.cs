
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{
    public abstract class GenericNodeBase<T> : NodeBase where T : class
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

        public override string SerializeJson() => JsonConvert.SerializeObject(FieldsInfo);
        public override void DeserializeJson(string json)
        {
            T info = JsonConvert.DeserializeObject<T>(json);
            fieldsInfo = info;
        }


    }
}