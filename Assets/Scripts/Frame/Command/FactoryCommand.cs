using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class CreateCharacterCommand : AbstractCommand
    {
        string CharacterName;
        CharacterFactory characterFactory;
        // Start is called before the first frame update
        public CreateCharacterCommand(string Name,CharacterFactory factory)
        {
            CharacterName = Name;
            characterFactory = factory;
        }
        protected override void OnExecute()
        {
            UnitBase character = CreateCharacter(CharacterName);
            characterFactory.SetPlayer = character.gameObject;
        }

        public UnitBase CreateCharacter(string Name)
        {
            Dictionary<string, CharacterPool> CharacterPlayerPool = ResManager.Intance.CharacterPlayerPool;
            Dictionary<string, CharacterInfo> CharacterBaseInfo =  ResManager.Intance.CharacterBaseInfo;
            //BattleUnit Unit;
            UnitBase player;
            GameObject Character = CharacterPlayerPool[Name].Get();
            
            //加载角色信息
            if(Character.GetComponent<UnitBase>() ==null)
            {
                UnitBase unit;
                unit = Character.AddComponent<UnitBase>();
                unit.Attr.Name = Name;
                unit.Attr.baseName = Name;
                unit.Attr.Hp = CharacterBaseInfo[Name].Hp;
                unit.Attr.CurrentHp = CharacterBaseInfo[Name].CurrentHP;
                unit.Military.MilitaryName = CharacterBaseInfo[Name].Type;
                unit.Military.MoveRange = CharacterBaseInfo[Name].MoveRange;
                if(CharacterBaseInfo[Name].Type == "英雄")
                {
                    unit.properties = new MasterProperties();
                }
                else
                {
                    unit.properties = new SolderProperties();
                }
                player = unit;
   
            }
            else
            {
                player = Character.GetComponent<UnitBase>();
            }
            Collider2D colliderbox = player.gameObject.GetComponent<Collider2D>();
            colliderbox.Disable();
            return player;
        }
    }
    public class PlaceCharacterCommand: AbstractCommand<bool>
    {
        protected override bool OnExecute()
        {
            IGameModel GameData = this.GetModel<IGameModel>();
            IInputSystem InputSys = this.GetSystem<IInputSystem>();
            int id = MapManager.World2Index(InputSys.handle.InputVector3,GameData.mapInfo.WidthLen,GameData.mapInfo.HeightLen);
            if(id != -1 && !GameData.mapInfo.MapTiles[id].ColiderBox)
            {
                return true;
            }
            return false;
        }
    }
}
