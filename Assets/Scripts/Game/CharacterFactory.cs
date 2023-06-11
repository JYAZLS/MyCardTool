using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class CharacterFactory : MonoBehaviour,IController
    {
        public IGameModel GameData;
        public IInputSystem InputSys;
        public IUISystem UISys;
        public GameEvent EventMgr;
        public GameObject SetPlayer;
        void Start()
        {
            GameData = this.GetModel<IGameModel>();
            InputSys = this.GetSystem<IInputSystem>();
            UISys    = this.GetSystem<IUISystem>();
        }
        public void Updated()
        {
            Vector3 worldpos = InputSys.handle.InputVector3;
            if(SetPlayer != null)
            {
                DragCharacter(SetPlayer,worldpos);
                if(Input.GetMouseButtonDown(1))
                {
                    SetPlayer = null;
                }
                if(Input.GetMouseButtonDown(0))
                {
                    bool flag = this.SendCommand<bool>(new PlaceCharacterCommand());
                    if(flag)
                    {
                        UnitBase info = SetPlayer.GetComponent<UnitBase>();
                        Collider2D colliderbox = info.gameObject.GetComponent<Collider2D>();
                        colliderbox.enabled = true;
                        //添加人物信息到系统
                        info.id = GameID.AllocateID();
                        info.Team = GameData.battleInfo.CurrentNumber;
                        GameData.playerInfo.characterInfo.Add(info.id,info);
                        GameID.registerID(info.id);
                        SetPlayer = null;
                        UISys.OpenUI("CreateCharacterMenu"); 
                    }
                }
            }
            else
            {
                if(Input.GetMouseButtonDown(0) && (InputSys.handle.currentBase != null) && (GameData.battleInfo.EditorMode))
                {
                    UnitBase unit = InputSys.handle.currentBase;
                    TypeEventSystem.Global.Send(new EventManager.ShowPlayerCommandMenu(){
                        Name = unit.Attr.Name,
                        Type = unit.Military.MilitaryName,
                        Hp = unit.Attr.Hp,
                        CurrentHp =unit.Attr.CurrentHp,
                        Team = unit.Team,
                        commmandlist = InputSys.handle.currentBase.properties.GetCommandBaseList()
                    });
                }
            }
        }
        public void DragCharacter(GameObject hero,Vector3 vector3)
        {
            //hero.transform.position = new Vector3(vector3.x + 0.24f, vector3.y + 0.24f, 0);
            hero.transform.position = vector3;
        }
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }

}