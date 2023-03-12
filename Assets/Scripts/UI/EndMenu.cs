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
            //�ҵ����еİ�ť��ӵ���¼�
            AddButtonListener(ref Buttons);
        }

        public override void AddButtonListener(ref Button[] buttons)
        {
            foreach (Button button in buttons)//������ӵ����ť�¼�
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
