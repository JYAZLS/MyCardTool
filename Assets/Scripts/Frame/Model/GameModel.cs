using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CardGameApp
{
    public interface IGameModel : IModel
    {
        public MapInfo mapInfo {get;set;}
        public BattleInfo battleInfo {get;set;}
        public PlayerInfo playerInfo {get;set;}
    }
    public class GameModel : AbstractModel, IGameModel
    {
        public MapInfo mapInfo {get;set;}
        public BattleInfo battleInfo {get;set;}
        public PlayerInfo playerInfo {get;set;}
        protected override void OnInit()
        {
            mapInfo = new();
            battleInfo = new();
            playerInfo = new();
            mapInfo.MapTiles = new();
            playerInfo.characterInfo = new();
            battleInfo.CurrentNumber = new();
            battleInfo.RoundNum = new();
        }
    }

    public class MapInfo 
    {
        public string Name;
        public int WidthLen;
        public int HeightLen;
        public List<GridInfo> MapTiles;
    }

    public class PlayerInfo
    {
        public Dictionary<int,CharacterInfo> characterInfo;
    }
    public class BattleInfo
    {
        public int TotalTeamNumber;
        public BindableProperty<int> RoundNum;  
        public BindableProperty<int> CurrentNumber;
    }
    public struct CharacterInfo
    { 
        public string Name;
        public string Type;
        public string BaseName;
        public int Hp;
        public int CurrentHP;
        public int MoveRange;
        public int AttackRange;
        public Dictionary<string,int> status;
        public Transform transform;
    }
}

