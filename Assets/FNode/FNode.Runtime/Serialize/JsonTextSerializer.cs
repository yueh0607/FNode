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
            //在需要字段严格匹配时使用
            new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Error,
            }
            ,
            //在不需要字段严格匹配时使用
            new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            },


            new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            },

            new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                MissingMemberHandling = MissingMemberHandling.Error,
            }
        };


        public override T Deserialize<T>(string str, int settingGroup = 0)
        {
            if (string.IsNullOrEmpty(str))
                return Activator.CreateInstance<T>();
            return JsonConvert.DeserializeObject<T>(str, SettingGroups[settingGroup]);
        }

        public override object DeserializeObject(string str,Type type, int settingGroup = 0)
        {
            if (string.IsNullOrEmpty(str))
                return Activator.CreateInstance(type);

            return JsonConvert.DeserializeObject(str, type, SettingGroups[settingGroup]);
        }

        public override void MapToObject(string str, object obj, int settingGroup = 0)
        {
            if(string.IsNullOrEmpty(str))
                return;
            JsonConvert.PopulateObject(str, obj, SettingGroups[settingGroup]);
        }

        public override string Serialize<T>(T obj, int settingGroup = 0)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, SettingGroups[settingGroup]);
        }

        public override string SerializeObject(object obj, Type type, int settingGroup = 0)
        {
           return JsonConvert.SerializeObject(obj, type, Formatting.Indented, SettingGroups[settingGroup]);
        }
    }
}
