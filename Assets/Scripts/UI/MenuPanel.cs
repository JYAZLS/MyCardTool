using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CardGameApp
{
    public class MenuPanel : BasePanel
    {
        public UnityAction NewGameOnClickCallBack;
        public UnityAction OptionGameOnClickCallBack;
        public UnityAction ExitGameOnClickCallBack;
        public override void OnEnter()
        {
            //base.OnEnter();
            Button[] Buttons = UnityEngine.Object.FindObjectsOfType<Button>();
            //�ҵ����еİ�ť��ӵ���¼�
            AddButtonListener(ref Buttons);
        }

        public void NewGameOnClick()
        {
            NewGameOnClickCallBack?.Invoke();
            //SceneController.Intance.SetState(new SetGame(SceneController.Intance), "SetGame");
        }
        public void OptionGameOnClick()
        {
            OptionGameOnClickCallBack?.Invoke();
        }
        public void ExitGameOnClick()
        {
            ExitGameOnClickCallBack?.Invoke();
        }

    }
}
