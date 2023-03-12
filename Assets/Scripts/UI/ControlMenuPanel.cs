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
        IBattleSystem battleSystem;
        public Dictionary<string, TextMeshProUGUI> TextViews = new ();
        
        public override void OnEnter()
        {
            battleSystem = this.GetSystem<IBattleSystem>();
            TextMeshProUGUI[] Texts = UnityEngine.Object.FindObjectsOfType<TextMeshProUGUI>();
            foreach(var it in Texts)
            {
                TextViews.Add(it.name,it);
            }
            TextViews["Team"].text = "Team:" + battleSystem.TeamNum.ToString();
            TextViews["Round"].text = "Round:" + battleSystem.RoundNum.ToString();

            battleSystem.TeamNum.Register(newValue =>{
                TextViews["Team"].text = "Team:" + newValue.ToString();
            }).UnRegisterWhenGameObjectDestroyed(TextViews["Team"].gameObject);
            battleSystem.RoundNum.Register(newValue =>{
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
