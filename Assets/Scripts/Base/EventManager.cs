using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace CardGameApp
{
    public class EventManager : MonoBehaviour,IController
    {
        public struct ShowPlayerCommandMenu
        {
            public string Name;
            public string Type;
            public int Hp;
            public int CurrentHp;
            public int Team;
            public string[] commmandlist;
        }
        void Start()
        {
            IGameModel GameData = this.GetModel<IGameModel>();
            IUISystem UISys = this.GetSystem<IUISystem>();//获取UI系统
            IMapSystem MapSys = this.GetSystem<IMapSystem>();//地图系统
            IInputSystem InputSys = this.GetSystem<IInputSystem>();
            GameEvent EventSys = this.GetSystem<GameEvent>();


            TypeEventSystem.Global.Register<ShowPlayerCommandMenu>(e=>
            {
                CommandView view= (CommandView)UISys.GetPanel("CommandMenu");
                view.InputFieldViews["NameInput"].text = e.Name;
                view.InputFieldViews["HPInputField"].text = e.Hp.ToString() +"/" + e.CurrentHp.ToString(); 
                view.InputFieldViews["TeamInputField"].text =  e.Team.ToString();
                view.ClearButtonList();
                view.GenerateButtonList(view.ScrollViews["CommandScroll"].transform,e.commmandlist);
                UISys.OpenUI("CommandMenu");
            }).UnRegisterWhenGameObjectDestroyed(this);
        }

        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }

}
