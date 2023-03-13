using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace CardGameApp
{
    public class SetGameCommand : AbstractCommand
    {
        int _PlayerCount;
        string _MapName;
        public SetGameCommand(int PlayerCount,string MapName)
        {
            _PlayerCount = PlayerCount;
            _MapName = MapName;
        }
        protected override void OnExecute()
        {
            Dictionary<string, MapInfo> MapBaseInfo = ResManager.Intance.MapBaseInfo;
            IGameModel mModel = this.GetModel<IGameModel>();
            IMapSystem mapSystem = this.GetSystem<IMapSystem>();
            IBattleSystem battleSystem = this.GetSystem<IBattleSystem>();
            if (_PlayerCount <2) _PlayerCount = 2;
            else if(_PlayerCount>9) _PlayerCount= 9;
            mModel.playerInfo.PlayerTeamNumber = _PlayerCount;//队伍数量
            mModel.mapInfo.Name = _MapName;//地图名
            ResLoader resLoader = ResManager.Intance.mResLoader;
            var mapprefab = resLoader.LoadSync<GameObject>("map",mModel.mapInfo.Name);
            GameObject Grid = mapprefab.Instantiate();
            Grid.name = mModel.mapInfo.Name;
            mModel.mapInfo.WidthLen = MapBaseInfo[mModel.mapInfo.Name].WidthLen;
            mModel.mapInfo.HeightLen = MapBaseInfo[mModel.mapInfo.Name].HeightLen;

            mapSystem.Tilemaps = Grid.GetComponentInChildren<Tilemap>();

            var MapJson = resLoader.LoadSync<TextAsset>("mapjson",mModel.mapInfo.Name);
            string MapInfo = MapJson.ToString();
            ReadMapInfo(MapInfo);     
            // Debug.Log(mapSystem.TileInfo.Count);
            // Debug.Log(mapSystem.TileInfo[1].id);
            // Debug.Log(mapSystem.TileInfo[1].Name);
            // Debug.Log(mapSystem.TileInfo[1].GridType);
            // Debug.Log(mapSystem.TileInfo[1].Colider);
            // resLoader.Recycle2Cache();
            // resLoader = null;
        }

        public void ReadMapInfo(string JsonStream)
        {
            IMapSystem mapSystem = this.GetSystem<IMapSystem>();
            JObject obj = JObject.Parse(JsonStream);
            JArray Tiles = JArray.Parse(obj["tiles"].ToString());
            foreach(var it in Tiles)
            {
                GridInfo info = new GridInfo();
                info.id = (int)float.Parse(it["id"].ToString());
                JArray properties = JArray.Parse(it["properties"].ToString());
                foreach(var property in properties)
                {
                    JObject cell = JObject.Parse(property.ToString());
                    if(cell.Value<string>("name") == "Colider")
                    {
                        info.ColiderBox = cell.Value<bool>("value");
                    }
                    else if(cell.Value<string>("name") == "Name")
                    {
                        info.Name = cell.Value<string>("value");
                    }
                    else if(cell.Value<string>("name") == "Type")
                    {
                        info.GridType = cell.Value<int>("value");
                    }
                }
                mapSystem.TileInfo.Add(info);
            }
        }


    }
}
