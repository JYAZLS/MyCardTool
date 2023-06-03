using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CardGameApp
{
    
    public class ControlMenuPanel : BasePanel,IController
    {
        IGameModel GameData;
        public Dictionary<string, TextMeshProUGUI> TextViews = new ();
        
        public override void OnEnter()
        {
            GameData = this.GetModel<IGameModel>();
            TextMeshProUGUI[] Texts = UnityEngine.Object.FindObjectsOfType<TextMeshProUGUI>();
            foreach(var it in Texts)
            {
                Debug.Log(it.name);
                TextViews.Add(it.name,it);
            }
            TextViews["Team"].text = "Team:" + GameData.battleInfo.CurrentNumber.Value.ToString();
            TextViews["Round"].text = "Round:" + GameData.battleInfo.RoundNum.Value.ToString();

            GameData.battleInfo.CurrentNumber.Register(newValue =>{
                TextViews["Team"].text = "Team:" + newValue.ToString();
            }).UnRegisterWhenGameObjectDestroyed(TextViews["Team"].gameObject);
            GameData.battleInfo.RoundNum.Register(newValue =>{
                TextViews["Round"].text = "Round:" + newValue.ToString();
            }).UnRegisterWhenGameObjectDestroyed(TextViews["Round"].gameObject);
        }
        public override void OnExit()
        {
        }
        public override void OnResume()
        {
        }
        public override void OnPasue()
        {
        }
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}
