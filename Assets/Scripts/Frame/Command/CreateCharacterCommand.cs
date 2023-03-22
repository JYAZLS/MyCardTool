using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class CreateCharacterCommand : AbstractCommand
    {
        string CharacterName;
        // Start is called before the first frame update
        public CreateCharacterCommand(string Name)
        {
            CharacterName = Name;
        }
        protected override void OnExecute()
        {
            IBattleSystem battleSystem = this.GetSystem<IBattleSystem>();
            IUISystem UISystem = this.GetSystem<IUISystem>();
            //gameSystem.CharacterPlayerPool[Name].Get();
            ICharacter gameobject = CreateCharacter(CharacterName);
            battleSystem.Hero = gameobject;
            UISystem.PopPanel();
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
}
