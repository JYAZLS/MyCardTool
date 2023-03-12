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
        private IGameSystem GameSystem;
        private IBattleSystem BattleSystem;
        private bool SettingMode = false;
        private ICharacter PtrCharacter = null;
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

            UISystem = this.GetSystem<IUISystem>();//获取UI系统
            MapSystem = this.GetSystem<IMapSystem>();//地图系统
            GameSystem = this.GetSystem<IGameSystem>();//游戏基础数据
            BattleSystem = this.GetSystem<IBattleSystem>();//人物系统
            UISystem.CreatePanel("ControlMenuPanel", UI_ControlMenu);
            UISystem.CreatePanel("CreateCharacterMenu", UI_CreateCharacterMenu);
            UISystem.CreatePanel("EndMenu",UI_EndMenu);
            UISystem.CreatePanel("CommandMenu",UI_CommandView);

            UISystem.PopPanel();
            UISystem.PopPanel();

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
            
            SettingMode = true;
        }

        public override void StateUpdate()
        {
            
            MapSystem.Updated();
            ICharacter battle  = null;
            //画布判断，避免多层点击
            if(UISystem.GetCurrentPanelName() != "CreateCharacterMenu")
            {
                battle = null;
            }
            else
            {
                battle =  BattleSystem.GetCharacter(MapSystem.CursorVecter);
            }
            if(PtrCharacter != null)
            {
                MapSystem.ShowMovePath(PtrCharacter.mGameObject.transform.position,MapSystem.CursorVecter);
            }
            if (SettingMode)
            {
                if (BattleSystem.ChooseCharacter!=null)
                {
                    BattleSystem.DragCharacter(MapSystem.CursorVecter);

                    if (Input.GetMouseButtonDown(0))//左键点击
                    {
                        BattleSystem.PlaceCharacter();
                        UISystem.PushPanel("CreateCharacterMenu", UI_CreateCharacterMenu);
                    }
                    if (Input.GetMouseButtonDown(1))//右键点击
                    {
                        BattleSystem.ReleaseCharacter();
                        UISystem.PushPanel("CreateCharacterMenu", UI_CreateCharacterMenu);
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))//左键点击
                    {
                        if(battle != null)
                        {
                            PtrCharacter = battle;
                            MapSystem.ShowMoveRange(PtrCharacter.mGameObject.transform,4,PtrCharacter.Military.MilitaryName,PtrCharacter.Team);
                            // UISystem.PushPanel("CommandMenu",UI_CommandView);
                            // UI_CommandView.InputFieldViews["NameInput"].text = battle.CharacterAttr.Name;
                            // UI_CommandView.InputFieldViews["HPInputField"].text = battle.CharacterAttr.Hp.ToString() +"/" + battle.CharacterAttr.CurrentHp.ToString(); 
                            // UI_CommandView.InputFieldViews["TeamInputField"].text = battle.Team.ToString();
                            // UI_CommandView.GenerateButtonList(UI_CommandView.ScrollViews["CommandScroll"].transform,PtrCharacter.GetCommandBaseList());
                        }
                    }
                    if (Input.GetMouseButtonDown(1))//右键点击
                    {
                        PtrCharacter = null;
                        MapSystem.ClearMoveRange();
                        //UISystem.PushPanel("EndMenu",UI_EndMenu);
                    }
                }   
            }
            else
            { 
            }
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
                this.SendCommand(new CreateCharacterCommand(button));
            }
        }
        
        private void ActionMenuButtonClickHandle(string button)
        {
            if(button == "Next")
            {
                this.SendCommand<TeamChangeCommand>();
            }
            else if(button == "Cancel")
            {

            }
            else
            {

            }
            UISystem.PopPanel();
        }

        private void CommandClickHandle(string cmd)
        {
            if(CommandRoot.Count == 0)
            {
                if(cmd == "Cancel")
                {
                    UI_CommandView.ClearButtonList();
                    UISystem.PopPanel();   
                }
                else if(cmd == "Delete")
                {
                    BattleSystem.DeleteCharacter(PtrCharacter);
                    PtrCharacter = null;
                    UI_CommandView.ClearButtonList();
                    UISystem.PopPanel();
                }
                else if(cmd == "ChangeType")
                {
                    UI_CommandView.ClearButtonList();
                    UISystem.PopPanel();
                    //CommandRoot.Add("ChangeType");
                    //UI_CommandView.GenerateButtonList(UI_CommandView.ScrollViews["CommandScroll"].transform,);
                }
                else if(cmd == "Wait")
                {
                    UI_CommandView.ClearButtonList();
                    UISystem.PopPanel();
                }
            }
            
        }

        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}
