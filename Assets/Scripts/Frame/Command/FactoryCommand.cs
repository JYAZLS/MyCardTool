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
            ICharacter character = CreateCharacter(CharacterName);
            characterFactory.SetPlayer = character.mGameObject;
        }

        public ICharacter CreateCharacter(string Name)
        {
            Dictionary<string, CharacterPool> CharacterPlayerPool = ResManager.Intance.CharacterPlayerPool;
            Dictionary<string, CharacterInfo> CharacterBaseInfo =  ResManager.Intance.CharacterBaseInfo;
            //BattleUnit Unit;
            ICharacter character;
            GameObject Character = CharacterPlayerPool[Name].Get();
            
            //加载角色信息
            if(Character.GetComponent<ICharacter>() ==null)
            {
                if(CharacterBaseInfo[Name].Type == "英雄")
                {
                    Master Unit;
                    Unit = Character.AddComponent<Master>();
                    Unit.mGameObject = Character;
                    Unit.CharacterAttr.Name = Name;
                    Unit.CharacterAttr.baseName = Name;
                    Unit.CharacterAttr.Hp = CharacterBaseInfo[Name].Hp;
                    Unit.CharacterAttr.CurrentHp = CharacterBaseInfo[Name].CurrentHP;
                    Unit.Military.MilitaryName = CharacterBaseInfo[Name].Type;
                    Unit.Military.MoveRange = CharacterBaseInfo[Name].MoveRange;
                    character = (ICharacter)Unit;
                }
                else
                {
                    Solder Unit;
                    Unit = Character.AddComponent<Solder>();
                    Unit.mGameObject = Character;
                    Unit.CharacterAttr.Name = Name;
                    Unit.CharacterAttr.baseName = Name;
                    Unit.CharacterAttr.Hp = CharacterBaseInfo[Name].Hp;
                    Unit.CharacterAttr.CurrentHp = CharacterBaseInfo[Name].CurrentHP;
                    Unit.Military.MilitaryName = CharacterBaseInfo[Name].Type;
                    Unit.Military.MoveRange = CharacterBaseInfo[Name].MoveRange;
                    character = (ICharacter)Unit;
                }     
            }
            else
            {
                character = Character.GetComponent<ICharacter>();
            }
            Collider2D colliderbox = character.mGameObject.GetComponent<Collider2D>();
            colliderbox.Disable();
            return character;
        }
    }
    public class PlaceCharacterCommand: AbstractCommand<bool>
    {
        protected override bool OnExecute()
        {
            IGameModel GameData = this.GetModel<IGameModel>();
            int id = MapManager.World2Index(InputHandle.Intance.InputVector3,GameData.mapInfo.WidthLen,GameData.mapInfo.HeightLen);
            if(id != -1 && !GameData.mapInfo.MapTiles[id].ColiderBox)
            {
                return true;
            }
            return false;
        }
    }
}
