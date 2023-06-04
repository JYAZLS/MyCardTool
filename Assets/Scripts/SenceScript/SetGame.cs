using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace CardGameApp
{
    public class SetGame : ISceneState,IController
    {
        private SetGamePanel UI_Panel;
        private IUISystem UISystem;
        public SetGame(SceneController controller) : base(controller)
        {
            this.StateName = "SetGame";
        }

        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }

        public override void StateBegin()
        {
            Debug.Log("Menu StateBegin");
            UI_Panel = new SetGamePanel();
            UISystem = this.GetSystem<IUISystem>();//获取系统管理器
            UISystem.CreatePanel("SetGamePanel", UI_Panel);
            //UISystem.OpenUI("SetGamePanel");
            this.SendCommand(new OpenUI("SetGamePanel"));
            //设置默认值
            UI_Panel.InputFields["InputField"].text = "2";
            //获取数据
            List<string> map_options = ResManager.Intance.MapBaseInfo.Keys.ToList<string>();
            UI_Panel.LoadDropDownOption(UI_Panel.DropDowns["Dropdown"], map_options);//地图选项

            UI_Panel.StartGameOnClickCallBack += () =>
            {
                int playercount = (int)float.Parse(UI_Panel.InputFields["InputField"].text);//队伍数量
                string mapname = UI_Panel.DropDowns["Dropdown"].captionText.text;//地图名
                this.SendCommand(new SetGameCommand(playercount, mapname));
                //准备切换场景
                 SceneController.Intance.SetState(new Game(SceneController.Intance), "Game");
                
            };
            /*            UI = GameObject.FindGameObjectWithTag("UI").GetComponent<SetGameUI>();

                        UI.StartGameButton.onClick.AddListener(() =>
                        {
                            int playercount= (int)float.Parse(UI.inputField.text);//队伍数量
                            string mapname = UI.dropdown.captionText.text;//地图名
                            this.SendCommand(new SetGameCommand(playercount, mapname));
                            //跳转场景
                        });*/
        }

        public override void StateUpdate()
        {

        }
        public override void StateEnd()
        {
            UISystem.PanelDestoryAll();
            UISystem.UIManager.DestoryUI("SetGamePanel");
        }
    }
}
