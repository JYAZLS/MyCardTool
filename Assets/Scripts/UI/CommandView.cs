using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using QFramework;
namespace CardGameApp
{
    public class CommandView : BasePanel,IController
    {
        public Dictionary<string,TMP_InputField>InputFieldViews = new();
        public Dictionary<string,GridLayoutGroup> ScrollViews = new();
        public UnityAction<string> ButtonClick;    
        private List<Button> buttonlist;
        public override void OnEnter()
        {
            buttonlist = new List<Button>();
            //base.OnEnter();
            Button[] Buttons = UnityEngine.Object.FindObjectsOfType<Button>();
            ScrollRect[] scrollRects = UnityEngine.Object.FindObjectsOfType<ScrollRect>();
            foreach (ScrollRect rect in scrollRects)
            {
                GridLayoutGroup group = rect.GetComponentInChildren<GridLayoutGroup>();
                ScrollViews.Add(rect.name, group);
            }
            TMP_InputField[] Inputfields = UnityEngine.Object.FindObjectsOfType<TMP_InputField>();
            foreach(TMP_InputField inputfield in Inputfields)
            {
                InputFieldViews.Add(inputfield.name,inputfield);
            }
            
            //找到所有的按钮添加点击事件
            //AddButtonListener(ref Buttons);
        }


        public void GenerateButtonList(Transform parent,string[] command)
        {
            ButtonPool buttonPool =  ResManager.Intance.ButtonPool; 
            foreach(var it in command)
            {
                Button button = buttonPool.Get();
                TextMeshProUGUI textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.text = it;
                button.transform.SetParent(parent);
                buttonlist.Add(button);
                button.onClick.AddListener(()=>{
                    ButtonClick?.Invoke(textMeshPro.text);
                });
            }
        }

        public void ClearButtonList()
        {
            GameObject ButtonPoolMgr = ResManager.Intance.ButtonPoolMgr;
            ButtonPool buttonPool =  ResManager.Intance.ButtonPool; 
            foreach(var it in buttonlist)
            {
                it.onClick.RemoveAllListeners();
                it.transform.SetParent(ButtonPoolMgr.transform);
                buttonPool.Release(it);
            }
            buttonlist.Clear();
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
