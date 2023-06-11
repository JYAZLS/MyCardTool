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
        public UnityAction<string> ButtonClick;
        public Dictionary<string, TextMeshProUGUI> TextViews = new ();
        
        public override void OnEnter()
        {
            GameData = this.GetModel<IGameModel>();
            //显示label
            TextMeshProUGUI[] Texts = UnityEngine.Object.FindObjectsOfType<TextMeshProUGUI>();
            foreach(var it in Texts)
            {
                TextViews.Add(it.name,it);
            }
            //添加按钮
            Button[] Buttons = UnityEngine.Object.FindObjectsOfType<Button>();
            AddButtonListener(ref Buttons);

            TextViews["Team"].text = "Team:" + GameData.battleInfo.CurrentNumber.Value.ToString();
            TextViews["Round"].text = "Round:" + GameData.battleInfo.RoundNum.Value.ToString();
            //
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
        public override void AddButtonListener(ref Button[] buttons)
        {
            foreach (Button button in buttons)//反射添加点击按钮事件
            {
                TextMeshProUGUI textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
                //Debug.Log(textMeshPro.text);
                button.onClick.AddListener(() =>
                {
                    ButtonClick?.Invoke(textMeshPro.text);
                });
            }
        }
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}
