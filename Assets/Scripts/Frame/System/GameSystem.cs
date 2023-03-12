using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardGameApp.LoadCharacterInfoEndCommand;
using static CardGameApp.LoadMapInfoEndCommand;

namespace CardGameApp
{
    public interface IGameSystem : ISystem
    {
        public string[] LoadCharacterInfo { get; set; }
        public bool CharacterInfoStatus { get;}     
        public string[] LoadMapInfo { get; set; }
        public bool MapInfoStatus { get; }
        public Dictionary<string, CharacterInfo> CharacterBaseInfo { get; }//存放地图基础数据
        public Dictionary<string, MapInfo> MapBaseInfo { get; }//存放地图基础数据
        public Dictionary<string, CharacterPool> CharacterPlayerPool { get; set; }
        public GameObject CharacterPoolMgr { get; }
        public ButtonPool ButtonPool { get; set;}
        public GameObject ButtonPoolMgr {get; }
        public PathPool   PathPool{get; set;}
        public GameObject PathPoolMgr { get; }
        public ICharacter CreateCharacter(string Name);
    }
    /// <summary>
    /// 游戏相关数据
    /// </summary>
    public class GameSystem : AbstractSystem, IGameSystem
    {
        public enum EnumMapData
        {
            Name,
            WidthLen,
            HeightLen,
        }
        public enum EnumCharacterData
        {
            Name,
            Type,
            HP,
            CurrenHP,
            MoveRange
        }
        public struct StartLoadCharacterPrefabEvent {};//启动加载预制体事件
        public Dictionary<string, CharacterInfo> CharacterBaseInfo { get; } = new Dictionary<string, CharacterInfo>();//人物基础数据
        public Dictionary<string, MapInfo> MapBaseInfo { get; } = new Dictionary<string, MapInfo>();//地图基础数据
        public bool CharacterInfoStatus { get; private set; } = false;//人物数据状态
        public string[] LoadCharacterInfo { get; set; }
        public bool MapInfoStatus { get; private set; } = false;//地图数据状态
        public string[] LoadMapInfo { get; set; }
        public Dictionary<string, CharacterPool> CharacterPlayerPool { get; set; } = new Dictionary<string, CharacterPool>();
        public GameObject CharacterPoolMgr { get; set; }
        public ButtonPool ButtonPool { get; set;}
        public GameObject ButtonPoolMgr { get; set; }
        public PathPool   PathPool {get; set;}
        public GameObject PathPoolMgr { get; set; }
        // Start is called before the first frame update
        protected override void OnInit()
        {
            CharacterPoolMgr = new GameObject("CharacterPoolMgr");
            ButtonPoolMgr = new GameObject("ButtonPoolMgr");
            ButtonPool = ButtonPoolMgr.AddComponent<ButtonPool>();
            PathPoolMgr = new GameObject("PathPoolMgr");
            PathPool = PathPoolMgr.AddComponent<PathPool>();
            //加载人物数据C
            this.RegisterEvent<LoadCharacterInfoEndEvent>(e =>
            {
                for (int i = 1; i < LoadCharacterInfo.Length; i++)
                {
                    string[] Data = LoadCharacterInfo[i].Split(',');
                    CharacterInfo characterInfo = new();
                    string Name = Data[(int)EnumCharacterData.Name];
                    characterInfo.Hp = (int)float.Parse(Data[(int)EnumCharacterData.HP]);
                    characterInfo.Type = Data[(int)EnumCharacterData.Type];
                    characterInfo.CurrentHP = (int)float.Parse(Data[(int)EnumCharacterData.CurrenHP]);
                    characterInfo.MoveRange = (int)float.Parse(Data[(int)EnumCharacterData.MoveRange]);
                    //加入数据
                    CharacterBaseInfo.Add(Name, characterInfo);
                }
                LoadCharacterInfo = null;
                CharacterInfoStatus = true;
                this.SendEvent<StartLoadCharacterPrefabEvent>();
            });

            //加载地图信息
            this.RegisterEvent<LoadMapInfoEndEvent>(e =>
            {
                for (int i = 1; i < LoadMapInfo.Length; i++)
                {
                    string[] Data = LoadMapInfo[i].Split(',');
                    MapInfo mapInfo = new() { };
                    mapInfo.Name = Data[(int)EnumMapData.Name];
                    mapInfo.WidthLen = (int)float.Parse(Data[(int)EnumMapData.WidthLen]);
                    mapInfo.HeightLen = (int)float.Parse(Data[(int)EnumMapData.HeightLen]);
                    MapBaseInfo.Add(mapInfo.Name, mapInfo);
                }
                LoadMapInfo = null;
                MapInfoStatus = true;
           
            });
        }

        public ICharacter CreateCharacter(string Name)
        {
            BattleUnit Unit;
            GameObject Character = CharacterPlayerPool[Name].Get();
            if(Character.GetComponent<BattleUnit>() == null)
            {
                //加载角色信息
                Unit = Character.AddComponent<BattleUnit>();

                Unit.CharacterInfo.mGameObject = Character;
                Unit.CharacterInfo.characterAttr.Name = Name;
                Unit.CharacterInfo.characterAttr.baseName = Name;
                Unit.CharacterInfo.characterAttr.Hp = CharacterBaseInfo[Name].Hp;
                Unit.CharacterInfo.characterAttr.CurrentHp = CharacterBaseInfo[Name].CurrentHP;
                Unit.CharacterInfo.military.MilitaryName = CharacterBaseInfo[Name].Type;
                Unit.CharacterInfo.military.MoveRange = CharacterBaseInfo[Name].MoveRange;
            }
            else
            {
                Unit =  Character.GetComponent<BattleUnit>();
            }
            ICharacter character = (ICharacter)Unit;
            return character;
        }
        public void ReleaseCharacter(string Name,GameObject _object)
        {
            CharacterPlayerPool[Name].Release(_object);
        }
    }
}
