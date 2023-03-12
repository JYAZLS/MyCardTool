using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace CardGameApp
{
    public class SetGamePanel : BasePanel
    {
        public Dictionary<string, TMP_InputField> InputFields = new() { };
        public Dictionary<string, TMP_Dropdown> DropDowns = new() { };
        public UnityAction StartGameOnClickCallBack;
        public override void OnEnter()
        {
            //base.OnEnter();
            Button[] Buttons = UnityEngine.Object.FindObjectsOfType<Button>();
            //找到所有的按钮添加点击事件
            AddButtonListener(ref Buttons);

            TMP_InputField[] inputFields = UnityEngine.Object.FindObjectsOfType<TMP_InputField>();
            //Debug.Log(inputFields.Length);
            foreach (var inputField in inputFields)
            {
                InputFields.Add(inputField.name, inputField);
            }

            TMP_Dropdown[] dropdowns = UnityEngine.Object.FindObjectsOfType<TMP_Dropdown>();
            //Debug.Log(dropdowns.Length);
            foreach (var dropdown in dropdowns)
            {
                DropDowns.Add(dropdown.name, dropdown);
            }

        }

        public void StartGameOnClick()
        {
            //Debug.Log("开始游戏");
            StartGameOnClickCallBack?.Invoke();
        }

        public void LoadDropDownOption(TMP_Dropdown _Dropdown, List<string> options)
        {
            List<TMP_Dropdown.OptionData> optionDatas = new() { };
            foreach (var item in options)
            {
                TMP_Dropdown.OptionData optionData = new() { text = item };
                optionDatas.Add(optionData);
            }
            _Dropdown.options = optionDatas;
        }

    }
}
