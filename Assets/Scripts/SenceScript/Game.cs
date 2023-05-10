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
            MapSystem = this.GetSystem<IMapSystem>();//地图系统
            BattleSystem = this.GetSystem<IBattleSystem>();//人物系统

            GameObject cursor = ResManager.Intance.Cursor.Instantiate<GameObject>();
            GameObject GameController = new GameObject("GameController");
            playerController = GameController.AddComponent<PlayerController>();
            characterFactory = GameController.AddComponent<CharacterFactory>();
            cursor.name = "Cursor";
            MapSystem.AddCursor(cursor);

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
            
            ProcessManager.SettingMode = true;
        }

        public override void StateUpdate()
        {
            MapSystem.Updated();
            MapSystem.CheckColider();
        }

        public override void StateEnd()
        {
        }

        private void CreateCharacterButtonClickHandle(string button)
        {
            if (button == "Edit")
            {
                if(ProcessManager.SettingMode)
                {
                    ProcessManager.SettingMode = false;
                    UISystem.PanelClearAll();
                    UISystem.OpenUI("ControlMenuPanel");
                }
                else
                {
                    ProcessManager.SettingMode = true;
                    UISystem.PanelClearAll();
                    UISystem.OpenUI("ControlMenuPanel");
                    UISystem.OpenUI("CreateCharacterMenu");
                }
                
            }
            else
            {
                this.SendCommand(new CreateCharacterCommand(button));
                ProcessManager.Status = ProcessStatus.SetCharacter; 
            }
        }
        
        private void ActionMenuButtonClickHandle(string button)
        {
            if(button == "Next")
            {
                this.SendCommand<TeamChangeCommand>();
                UISystem.PopPanel();
                if(ProcessManager.SettingMode)
                {
                    UISystem.OpenUI("CreateCharacterMenu");
                }
                else
                {
                    Debug.Log("Stop:"+ProcessManager.SettingMode);
                    UISystem.PanelClearAll();
                    UISystem.OpenUI("ControlMenuPanel");
                }
            }
            else if(button == "Cancel")
            {
                UISystem.PopPanel();
                if(ProcessManager.SettingMode)
                {
                    UISystem.OpenUI("CreateCharacterMenu");
                }
            }
            else
            {
                UISystem.PopPanel();
                if(ProcessManager.SettingMode)
                {
                    UISystem.OpenUI("CreateCharacterMenu");
                }
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
                    ProcessManager.Status = ProcessStatus.None;
                    PtrCharacter = null;   
                }
                else if(cmd == "Delete")
                {
                    BattleSystem.DeleteCharacter(PtrCharacter);
                    PtrCharacter = null;
                    UI_CommandView.ClearButtonList();
                    UISystem.PopPanel();
                    UISystem.OpenUI("CreateCharacterMenu");
                    ProcessManager.Status = ProcessStatus.None;
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
                    if(ProcessManager.SettingMode)
                    {
                        UISystem.OpenUI("CreateCharacterMenu");
                    }
                    ProcessManager.Status = ProcessStatus.None;
                    PtrCharacter = null;
                }
            }
            else if(CommandRoot[0] == "ChangeType")
            {
                this.SendCommand(new ChangeHeroType(PtrCharacter,cmd));
                CommandRoot.Clear();
                UISystem.PopPanel();
                if(ProcessManager.SettingMode)
                {
                    UISystem.OpenUI("CreateCharacterMenu");
                }               
                ProcessManager.Status = ProcessStatus.None;
                PtrCharacter = null;
            }
            
        }     
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}
