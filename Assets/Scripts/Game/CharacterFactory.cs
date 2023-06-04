using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class CharacterFactory : MonoBehaviour,IController
    {
        public GameObject SetPlayer;
        void Start()
        {
            
        }
        public void Updated()
        {
            Vector3 worldpos = InputHandle.Intance.InputVector3;
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
                        SetPlayer = null;
                        this.SendCommand(new OpenUI("CreateCharacterMenu")); 
                    }
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