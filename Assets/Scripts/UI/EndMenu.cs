using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace CardGameApp
{
    public class EndMenu : BasePanel
    {
        public UnityAction<string> ButtonClick;
        public override void OnEnter()
        {
            //base.OnEnter();
            Button[] Buttons = UnityEngine.Object.FindObjectsOfType<Button>();
            //找到所有的按钮添加点击事件
            AddButtonListener(ref Buttons);
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

    }
}
