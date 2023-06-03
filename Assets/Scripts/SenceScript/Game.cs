using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using static UnityEditor.PlayerSettings;
using Unity.Mathematics;
using UnityEngine.Rendering;
using UnityEngine.UI;
namespace CardGameApp
{
    public class Game : ISceneState,IController
    {
        private ControlMenuPanel UI_ControlMenu;
        private CreateCharacterMenu UI_CreateCharacterMenu;
        private EndMenu UI_EndMenu;
        private CommandView UI_CommandView;
        private IUISystem UISystem;
        private IMapSystem MapSystem;
        private IBattleSystem BattleSystem;
        private ICharacter PtrCharacter = null;
        private List<string> CommandRoot = new ();
        private PlayerController playerController;
        private CharacterFactory characterFactory;
        private MapManager MapMgr;
        public Game(SceneController controller): base(controller)
        {
            this.StateName = "Game";
        }

        public override void StateBegin()
        {
            //UI加载
            UI_ControlMenu = new ControlMenuPanel();
            UI_CreateCharacterMenu = new CreateCharacterMenu();
            UI_EndMenu = new EndMenu();
            UI_CommandView = new CommandView();

            UISystem = this.GetSystem<IUISystem>();//获取UI系统
            // MapSystem = this.GetSystem<IMapSystem>();//地图系统
            // BattleSystem = this.GetSystem<IBattleSystem>();//人物系统

            GameObject cursor = ResManager.Intance.Cursor.Instantiate<GameObject>();
            GameObject GameController = new GameObject("GameController");
            playerController = GameController.AddComponent<PlayerController>();
            characterFactory = GameController.AddComponent<CharacterFactory>();
            MapMgr       = GameController.AddComponent<MapManager>();
            cursor.name = "Cursor";
            // MapSystem.AddCursor(cursor);

            UISystem.CreatePanel("ControlMenuPanel", UI_ControlMenu);
            UISystem.CreatePanel("CreateCharacterMenu", UI_CreateCharacterMenu);
            UISystem.CreatePanel("EndMenu",UI_EndMenu);
            UISystem.CreatePanel("CommandMenu",UI_CommandView);

            UISystem.OpenUI("ControlMenuPanel");
            UISystem.OpenUI("CreateCharacterMenu");
            
            UI_CreateCharacterMenu.ButtonClick += CreateCharacterButtonClickHandle;
            UI_EndMenu.ButtonClick += ActionMenuButtonClickHandle;
            UI_CommandView.ButtonClick += CommandClickHandle;
            
            UI_CommandView.InputFieldViews["NameInput"].onEndEdit.AddListener((arg0)=>{
                if(PtrCharacter != null)
                {
                    PtrCharacter.CharacterAttr.Name = arg0;
                }
            });
            UI_CommandView.InputFieldViews["HPInputField"].onEndEdit.AddListener((arg0)=>{
                if(PtrCharacter != null)
                {
                    string[] info = arg0.Split('/');
                    PtrCharacter.CharacterAttr.Hp = (int)float.Parse(info[0]);
                    PtrCharacter.CharacterAttr.CurrentHp = (int)float.Parse(info[1]);
                }
            });
            UI_CommandView.InputFieldViews["TeamInputField"].onEndEdit.AddListener((arg0)=>{
                if(PtrCharacter != null)
                {
                    PtrCharacter.SetTeam((int)float.Parse(arg0));
                }
            });
            
        }

        public override void StateUpdate()
        {
            Vector3 mousePos = Input.mousePosition;
            UnityEngine.Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 pos = MapMgr.ChangeWorldToTilePos(worldpos);
            InputHandle.Intance.InputVector3 = pos;
            MapMgr.Updated();
            playerController.Updated();
            characterFactory.Updated();
        }

        public override void StateEnd()
        {
        }

        private void CreateCharacterButtonClickHandle(string button)
        {
            if (button == "Edit")
            {           
            }
            else
            {
                this.SendCommand(new CreateCharacterCommand(button,characterFactory));
            }
        }
        
        private void ActionMenuButtonClickHandle(string button)
        {
            if(button == "Next")
            {
                UISystem.PopPanel();
            }
            else if(button == "Cancel")
            {
                UISystem.PopPanel();
            }
            else
            {
                UISystem.PopPanel();
            }
            
            
        }

        private void CommandClickHandle(string cmd)
        {
            if(CommandRoot.Count == 0)
            {
                if(cmd == "Cancel")
                {
                    UI_CommandView.ClearButtonList();
                    UISystem.PopPanel();
                    UISystem.OpenUI("CreateCharacterMenu");
                    PtrCharacter = null;   
                }
                else if(cmd == "Delete")
                {
                    PtrCharacter = null;
                    UI_CommandView.ClearButtonList();
                    UISystem.PopPanel();
                    UISystem.OpenUI("CreateCharacterMenu");
                    PtrCharacter = null;

                }
                else if(cmd == "ChangeType")
                {
                    UI_CommandView.ClearButtonList();
                    UISystem.PopPanel();
                    CommandRoot.Add("ChangeType");
                    UISystem.OpenUI("CommandMenu");
                    UI_CommandView.GenerateButtonList(UI_CommandView.ScrollViews["CommandScroll"].transform,PtrCharacter.GetTypeCommandList());
                    
                }
                else if(cmd == "Wait")
                {
                    UI_CommandView.ClearButtonList();
                    UISystem.PopPanel();
                    PtrCharacter = null;
                }
            }
            else if(CommandRoot[0] == "ChangeType")
            {
                this.SendCommand(new ChangeHeroType(PtrCharacter,cmd));
                CommandRoot.Clear();
                UISystem.PopPanel();
                PtrCharacter = null;
            }
            
        }     
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}
