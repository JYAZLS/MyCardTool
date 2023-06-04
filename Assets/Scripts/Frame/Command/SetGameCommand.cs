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
        int PlayerCount;
        string MapName;
        public SetGameCommand(int _PlayerCount,string _MapName)
        {
            PlayerCount = _PlayerCount;
            MapName = _MapName;
        }
        protected override void OnExecute()
        {
            Dictionary<string, MapInfo> MapBaseInfo = ResManager.Intance.MapBaseInfo;
            IGameModel mModel = this.GetModel<IGameModel>();
            if(PlayerCount < 2 )
                PlayerCount = 2;
            else if(PlayerCount>9) 
                PlayerCount= 9;

            mModel.battleInfo.TotalTeamNumber = PlayerCount;//队伍数量
            mModel.mapInfo.Name = MapName;//地图名
            ResLoader resLoader = ResManager.Intance.mResLoader;
            var mapprefab = resLoader.LoadSync<GameObject>("map",mModel.mapInfo.Name);
            GameObject Grid = mapprefab.Instantiate();
            Grid.name = mModel.mapInfo.Name;
            mModel.mapInfo.WidthLen = MapBaseInfo[mModel.mapInfo.Name].WidthLen;
            mModel.mapInfo.HeightLen = MapBaseInfo[mModel.mapInfo.Name].HeightLen;

            // mapSystem.Tilemaps = Grid.GetComponentInChildren<Tilemap>();

            var MapJson = resLoader.LoadSync<TextAsset>("mapjson",mModel.mapInfo.Name);
            string MapInfo = MapJson.ToString();
            List<ReadJsonTool.JsonData> mData = new();
            ReadJsonTool.ReadMapInfo(MapInfo,ref mData);
            foreach(var it in mData)
            {
                GridInfo info = new();
                info.id = it.id;
                info.Name = it.Name;
                info.GridType = it.GridType;
                info.ColiderBox = it.ColiderBox;
                mModel.mapInfo.MapTiles.Add(info);
            }
        }
    }

    public class SetNextTeam : AbstractCommand
    {
        protected override void OnExecute()
        {
            IGameModel mModel = this.GetModel<IGameModel>();
            mModel.battleInfo.CurrentNumber.Value++;
            if(mModel.battleInfo.CurrentNumber >= mModel.battleInfo.TotalTeamNumber)
            {
                mModel.battleInfo.CurrentNumber.Value = 0;
            }
        }
    }
}
