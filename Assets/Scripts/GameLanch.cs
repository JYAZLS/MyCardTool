using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using static UnityEditor.Progress;

namespace CardGameApp
{
    public class GameLanch : MonoBehaviour,IController
    {
        private void Awake()
        {
            this.gameObject.AddComponent<SceneController>();
            //资源管理器
            GameObject ResMgr = new GameObject("ResManager");
            ResMgr.AddComponent<ResManager>();
            //动作管理器
            GameObject ActionMgr = new GameObject("ActionManager");
            ActionMgr.AddComponent<ActionManager>(); 
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        void Start()
        {
            ActionKit.Delay(0.6f,()=>{
                SceneController.Intance.SetState(new Menu(SceneController.Intance), "Menu");
            }).Start(ActionManager.Intance);             
        }
        // Update is called once per frame
        void Update()
        {
            SceneController.Intance.StateUpdate();
        }
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}

