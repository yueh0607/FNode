using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{
    public class JsonTextSerializer : AbstractTextSerializer<JsonTextSerializer>
    {

        public override T Deserialize<T>(string str)
        {
            if (string.IsNullOrEmpty(str)) 
                return Activator.CreateInstance<T>();
            return JsonConvert.DeserializeObject<T>(str);
        }

        public override string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
