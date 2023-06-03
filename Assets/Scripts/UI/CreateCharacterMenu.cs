using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CardGameApp
{
    public class CreateCharacterMenu : BasePanel,IController
    {
        public UnityAction<string> ButtonClick;
        public Dictionary<string, GridLayoutGroup> ScrollViews = new();
        private Button[] Buttons;
        public override void OnEnter()
        {
            Dictionary<string, CharacterPool> CharacterPlayerPool = ResManager.Intance.CharacterPlayerPool;
            ButtonPool buttonPool =  ResManager.Intance.ButtonPool;

            ScrollRect[] scrollRects = UnityEngine.Object.FindObjectsOfType<ScrollRect>();
            foreach (ScrollRect rect in scrollRects)
            {
                GridLayoutGroup group = rect.GetComponentInChildren<GridLayoutGroup>();
                ScrollViews.Add(rect.name, group);
            }

            foreach (var it in CharacterPlayerPool.Keys)
            {
                Button button = buttonPool.Get();
                button.GetComponentInChildren<TextMeshProUGUI>().text = it;
                button.name = it;
                button.transform.SetParent(ScrollViews["CharacterScrollView"].transform);
            }

            Buttons = UnityEngine.Object.FindObjectsOfType<Button>();
            AddButtonListener(ref Buttons);
        }

        public override void OnPasue()
        {
            //Í£Ö¹¼àÌý
            PauseListenerClick(ref Buttons);
        }

        public override void OnResume()
        {
            //»Ö¸´¼àÌý
            AddButtonListener(ref Buttons);
        }

        public override void OnDestroy()
        {
            GameObject ButtonPoolMgr = ResManager.Intance.ButtonPoolMgr;
            ButtonPool buttonPool =  ResManager.Intance.ButtonPool;
            foreach(var it in Buttons)
            {
                it.gameObject.transform.SetParent(ButtonPoolMgr.transform);
                buttonPool.Release(it);
            }
            base.OnDestroy();
        }

        public override void AddButtonListener(ref Button[] buttons)
        {
            foreach (Button button in buttons)//·´ÉäÌí¼Óµã»÷°´Å¥ÊÂ¼þ
            {
                TextMeshProUGUI textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
                //Debug.Log(textMeshPro.text);
                button.onClick.AddListener(() =>
                {
                    ButtonClick?.Invoke(textMeshPro.text);
                });
            }
        }

        private void PauseListenerClick(ref Button[] buttons)
        {
            foreach (Button button in buttons)//ÔÝÍ£°´Å¥ÊÂ¼þ
            {
                button.onClick.RemoveAllListeners();
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}
