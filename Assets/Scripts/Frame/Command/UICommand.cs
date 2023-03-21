using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace CardGameApp
{
    //œ‘ æ÷∏¡Ó¿∏
    public class UICommandMenu : AbstractCommand
    {
        CommandView Panel;
        ICharacter Character;
        public UICommandMenu(CommandView panel,ICharacter character)
        {
            Panel = panel;
            Character = character;
        }
        protected override void OnExecute()
        {
            IUISystem UISystem = this.GetSystem<IUISystem>();
            UISystem.PushPanel("CommandMenu",Panel);
            Panel.InputFieldViews["NameInput"].text = Character.CharacterAttr.Name;
            Panel.InputFieldViews["HPInputField"].text = Character.CharacterAttr.Hp.ToString() +"/" + Character.CharacterAttr.CurrentHp.ToString(); 
            Panel.InputFieldViews["TeamInputField"].text = Character.Team.ToString();
            Panel.GenerateButtonList(Panel.ScrollViews["CommandScroll"].transform,Character.GetCommandBaseList());
        }
    }

    public class ChangeHeroType : AbstractCommand
    {
        string typeName;
        ICharacter Character;
        public ChangeHeroType(ICharacter character,string typename)
        {
            Character = character;
            typeName = typename;
        }
        protected override void OnExecute()
        {
            Character.Military.MilitaryName = typeName;
            Character.Military.MoveRange = ResManager.Intance.CharacterBaseInfo[typeName].MoveRange;
        }
    }
}

