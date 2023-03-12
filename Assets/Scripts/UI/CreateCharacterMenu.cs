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
            IGameSystem gameSystem = this.GetSystem<IGameSystem>();
            ScrollRect[] scrollRects = UnityEngine.Object.FindObjectsOfType<ScrollRect>();
            foreach (ScrollRect rect in scrollRects)
            {
                GridLayoutGroup group = rect.GetComponentInChildren<GridLayoutGroup>();
                ScrollViews.Add(rect.name, group);
            }

            foreach (var it in gameSystem.CharacterPlayerPool.Keys)
            {
                Button button = gameSystem.ButtonPool.Get();
                button.GetComponentInChildren<TextMeshProUGUI>().text = it;
                button.name = it;
                button.transform.SetParent(ScrollViews["CharacterScrollView"].transform);
            }

            Buttons = UnityEngine.Object.FindObjectsOfType<Button>();
            AddButtonListener(ref Buttons);
        }

        public override void OnPasue()
        {
            //停止监听
            PauseListenerClick(ref Buttons);
        }

        public override void OnResume()
        {
            //恢复监听
            AddButtonListener(ref Buttons);
        }

        public override void OnDestroy()
        {
            IGameSystem gameSystem = this.GetSystem<IGameSystem>();
            foreach(var it in Buttons)
            {
                it.gameObject.transform.SetParent(gameSystem.ButtonPoolMgr.transform);
                gameSystem.ButtonPool.Release(it);
            }
            base.OnDestroy();
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

        private void PauseListenerClick(ref Button[] buttons)
        {
            foreach (Button button in buttons)//反射添加点击按钮事件
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
