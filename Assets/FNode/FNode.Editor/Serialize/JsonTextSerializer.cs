using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNode.Editor
{
    public class JsonTextSerializer : AbstractTextSerializer<JsonTextSerializer>
    {

        static List<JsonSerializerSettings> SettingGroups = new List<JsonSerializerSettings>()
        {
            new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Error,
            }
            ,
            new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            }
        };


        public override T Deserialize<T>(string str, int settingGroup = 0)
        {
            if (string.IsNullOrEmpty(str))
                return Activator.CreateInstance<T>();
            return JsonConvert.DeserializeObject<T>(str, SettingGroups[settingGroup]);
        }

        public override string Serialize<T>(T obj, int settingGroup = 0)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, SettingGroups[settingGroup]);
        }
    }
}
