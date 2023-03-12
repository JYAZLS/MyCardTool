using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardGameApp.LoadCharacterInfoEndCommand;

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
            IGameSystem gameSystem = this.GetSystem<IGameSystem>();
            IBattleSystem battleSystem = this.GetSystem<IBattleSystem>();
            IUISystem UISystem = this.GetSystem<IUISystem>();
            //gameSystem.CharacterPlayerPool[Name].Get();
            ICharacter gameobject = gameSystem.CreateCharacter(CharacterName);
            battleSystem.SetChooseCharacter(gameobject);
            UISystem.PopPanel();
        }
    }
}
