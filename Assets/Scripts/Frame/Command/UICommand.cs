using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace CardGameApp
{
    //显示指令栏
    public class UICommandMenu : AbstractCommand
    {
        CommandView Panel;
        UnitBase Character;
        public UICommandMenu(CommandView panel,UnitBase character)
        {
            Panel = panel;
            Character = character;
        }
        protected override void OnExecute()
        {
            IUISystem UISystem = this.GetSystem<IUISystem>();
            UISystem.OpenUI("CommandMenu");
            Panel.InputFieldViews["NameInput"].text = Character.Attr.Name;
            Panel.InputFieldViews["HPInputField"].text = Character.Attr.Hp.ToString() +"/" + Character.Attr.CurrentHp.ToString(); 
            Panel.InputFieldViews["TeamInputField"].text = Character.Team.ToString();
            Panel.ClearButtonList();
            Panel.GenerateButtonList(Panel.ScrollViews["CommandScroll"].transform,Character.properties.GetCommandBaseList());
        }
    }
    //改变人物类型
    public class ChangeHeroType : AbstractCommand
    {
        string typeName;
        UnitBase Character;
        public ChangeHeroType(UnitBase character,string typename)
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
    public class OpenUI: AbstractCommand
    {
        string UI;
        public OpenUI(string ui)
        {
            UI = ui;
        }
        protected override void OnExecute()
        {
            IUISystem UIsystem = this.GetSystem<IUISystem>();
            UIsystem.OpenUI(UI);
        }
    }
    public class CloseUI: AbstractCommand
    {
        protected override void OnExecute()
        {
            IUISystem UIsystem = this.GetSystem<IUISystem>();
            UIsystem.PopPanel();
        }
    }
    public class CloseStrPanel: AbstractCommand
    {
        string UI;
        public CloseStrPanel(string ui)
        {
            UI = ui;
        }
        protected override void OnExecute()
        {
            IUISystem UIsystem = this.GetSystem<IUISystem>();
            UIsystem.CloseStrPanel(UI);
        }
    }
}

