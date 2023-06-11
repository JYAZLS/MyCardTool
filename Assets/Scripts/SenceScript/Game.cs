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
        //注册系统
        private IGameModel   GameData;
        private IUISystem    UISys;
        private IMapSystem   MapSys;
        private IInputSystem InputSys;
        private GameEvent    EventSys;
        //管理实例
        private PlayerController playerController;
        private CharacterFactory characterFactory;
        private MapManager MapMgr;
        private InputHandle Inputhandle;
        private EventManager eventManager; 
        //UI
        private ControlMenuPanel UI_ControlMenu;
        private CreateCharacterMenu UI_CreateCharacterMenu;
        private EndMenu UI_EndMenu;
        private CommandView UI_CommandView;
        
        private UnitBase PtrCharacter = null;
        private List<string> CommandRoot = new ();
        
        
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

            //系统获取
            GameData = this.GetModel<IGameModel>();
            UISys = this.GetSystem<IUISystem>();//获取UI系统
            MapSys = this.GetSystem<IMapSystem>();//地图系统
            InputSys = this.GetSystem<IInputSystem>();
            EventSys = this.GetSystem<GameEvent>();
            // BattleSystem = this.GetSystem<IBattleSystem>();//人物系统

            //游戏相关控制器
            //GameObject cursor = ResManager.Intance.Cursor.Instantiate<GameObject>();
            GameObject GameController = new GameObject("GameController");
            playerController = GameController.AddComponent<PlayerController>();
            characterFactory = GameController.AddComponent<CharacterFactory>();
            MapMgr           = GameController.AddComponent<MapManager>();
            Inputhandle      = GameController.AddComponent<InputHandle>();
            eventManager     = GameController.AddComponent<EventManager>();
            MapSys.MapMgr    = MapMgr;
            InputSys.handle  = Inputhandle;
            EventSys.eventMgr = eventManager;
            //cursor.name = "Cursor";
            // MapSystem.AddCursor(cursor);

            UISys.CreatePanel("ControlMenuPanel", UI_ControlMenu);
            UISys.CreatePanel("CreateCharacterMenu", UI_CreateCharacterMenu);
            UISys.CreatePanel("EndMenu",UI_EndMenu);
            UISys.CreatePanel("CommandMenu",UI_CommandView);

            UISys.OpenUI("ControlMenuPanel");
            UISys.OpenUI("CreateCharacterMenu");
            
            //UI_ControlMenu.ButtonClick += ControlMe
            UI_CreateCharacterMenu.ButtonClick += CreateCharacterButtonClickHandle;
            UI_EndMenu.ButtonClick += ActionMenuButtonClickHandle;
            UI_CommandView.ButtonClick += CommandClickHandle;

            GameData.battleInfo.EditorMode.Value = true;
            GameData.battleInfo.EditorMode.Register(newvalue =>{
                if(newvalue)
                {
                    UISys.OpenUI("CreateCharacterMenu");
                }
                else
                {
                    UISys.CloseStrPanel("CreateCharacterMenu");
                }
            });

            UI_CommandView.InputFieldViews["NameInput"].onEndEdit.AddListener((arg0)=>{
                if(PtrCharacter != null)
                {
                    PtrCharacter.Attr.Name = arg0;
                }
            });
            UI_CommandView.InputFieldViews["HPInputField"].onEndEdit.AddListener((arg0)=>{
                if(PtrCharacter != null)
                {
                    string[] info = arg0.Split('/');
                    PtrCharacter.Attr.Hp = (int)float.Parse(info[0]);
                    PtrCharacter.Attr.CurrentHp = (int)float.Parse(info[1]);
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
            Inputhandle.currentBase = null;
            Inputhandle.gridInfo = null;
            Vector3 mousePos = Input.mousePosition;
            UnityEngine.Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 pos = MapMgr.ChangeWorldToTilePos(worldpos);
            Inputhandle.InputVector3 = pos;
            foreach(var it in GameData.playerInfo.characterInfo)
            {
                if(it.Value.transform.position == pos)
                {
                    //Debug.Log(it.Value.Attr.Name);
                    Inputhandle.currentBase = it.Value;
                    break;
                }
            }


            MapMgr.Updated();
            playerController.Updated();
            characterFactory.Updated();
        }

        public override void StateEnd()
        {
            UISys.PanelDestoryAll();
            UISys.DestroyUI("ControlMenuPanel");
            UISys.DestroyUI("CreateCharacterMenu");
            UISys.DestroyUI("EndMenu");
            UISys.DestroyUI("CommandMenu");
        }

        private void CreateCharacterButtonClickHandle(string button)
        {
            if (button == "Edit")
            {  
                GameData.battleInfo.EditorMode.Value = GameData.battleInfo.EditorMode.Value?false:true;
                // Debug.Log(GameData.battleInfo.EditorMode.Value);   
            }
            else
            {
                this.SendCommand(new CreateCharacterCommand(button,characterFactory));
                UISys.PopPanel();
            }
        }
        
        private void ActionMenuButtonClickHandle(string button)
        {
            if(button == "Next")
            {
                this.SendCommand<SetNextTeam>();
            }
            else if(button == "Cancel")
            {
            }
            else
            {
            }
            UISys.PopPanel();         
        }

        private void CommandClickHandle(string cmd)
        {
            if(CommandRoot.Count == 0)
            {
                if(cmd == "Cancel")
                {
                    UI_CommandView.ClearButtonList();
                    UISys.PopPanel();
                    UISys.OpenUI("CreateCharacterMenu");
                    PtrCharacter = null;   
                }
                else if(cmd == "Delete")
                {
                    PtrCharacter = null;
                    UI_CommandView.ClearButtonList();
                    UISys.PopPanel();
                    UISys.OpenUI("CreateCharacterMenu");
                    PtrCharacter = null;

                }
                else if(cmd == "ChangeType")
                {
                    UI_CommandView.ClearButtonList();
                    UISys.PopPanel();
                    CommandRoot.Add("ChangeType");
                    UISys.OpenUI("CommandMenu");
                    UI_CommandView.GenerateButtonList(UI_CommandView.ScrollViews["CommandScroll"].transform,PtrCharacter.properties.GetTypeCommandList());
                }
                else if(cmd == "Wait")
                {
                    UI_CommandView.ClearButtonList();
                    UISys.PopPanel();
                    PtrCharacter = null;
                }
            }
            else if(CommandRoot[0] == "ChangeType")
            {
                this.SendCommand(new ChangeHeroType(PtrCharacter,cmd));
                CommandRoot.Clear();
                UISys.PopPanel();
                PtrCharacter = null;
            }
            
        }     
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}
