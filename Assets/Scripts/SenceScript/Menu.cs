using QFramework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardGameApp
{
    public class Menu : ISceneState,IController
    {
        private MenuPanel UI_Panel;
        private IUISystem UISystem;
        public Menu(SceneController controller): base(controller)
        {
            this.StateName = "Menu";
        }
        public override void StateBegin()
        {
            Debug.Log("Menu StateBegin");
            UI_Panel = new MenuPanel();
            UISystem = this.GetSystem<IUISystem>();//获取系统管理器
            UISystem.CreatePanel("MenuPanel", UI_Panel);

            UI_Panel.NewGameOnClickCallBack += () => {
                SceneController.Intance.SetState(new SetGame(SceneController.Intance), "SetGame");
            };
        }

        public override void StateUpdate()
        {

        }

        public override void StateEnd()
        {
            UISystem.PanelClearAll();
            UISystem.UIManager.DestoryUI("MenuPanel");
        }

        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}
