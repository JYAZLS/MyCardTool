using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CardGameApp
{
    public class ReadJsonTool
    {
        public struct JsonData{
            public int id;
            public string Name;
            public bool ColiderBox;
            public int GridType;
        }
        public static void ReadMapInfo(string JsonStream,ref List<JsonData> mData)
        {
            JObject obj = JObject.Parse(JsonStream);
            JArray Tiles = JArray.Parse(obj["tiles"].ToString());
            foreach(var it in Tiles)
            {
                JsonData data = new JsonData();
                data.id = (int)float.Parse(it["id"].ToString());
                JArray properties = JArray.Parse(it["properties"].ToString());
                foreach(var property in properties)
                {
                    JObject cell = JObject.Parse(property.ToString());
                    if(cell.Value<string>("name") == "Colider")
                    {
                        data.ColiderBox = cell.Value<bool>("value");
                    }
                    else if(cell.Value<string>("name") == "Name")
                    {
                        data.Name = cell.Value<string>("value");
                    }
                    else if(cell.Value<string>("name") == "Type")
                    {
                        data.GridType = cell.Value<int>("value");
                    }
                }
                mData.Add(data);
            }
        }
    }
}
