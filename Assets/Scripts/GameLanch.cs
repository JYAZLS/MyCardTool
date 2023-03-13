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
            GameObject ResManager = new GameObject("ResManager");
            ResManager.AddComponent<ResManager>();           
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        void Start()
        {
            ActionKit.Delay(0.6f,()=>{
                SceneController.Intance.SetState(new Menu(SceneController.Intance), "Menu");
            }).Start(this);             
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

