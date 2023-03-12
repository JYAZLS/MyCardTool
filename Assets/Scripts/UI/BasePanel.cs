using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace CardGameApp
{
    /// <summary>
    /// UI面板的基类
    /// </summary>
    public abstract class BasePanel
    {
        public GameObject PanelObject;
        public virtual void OnEnter() { }
        public virtual void OnExit() => PanelObject.SetActive(false);
        public virtual void OnPasue() { }
        public virtual void OnDestroy() => GameObject.Destroy(PanelObject); 
        public virtual void OnResume() => PanelObject.SetActive(true);

        /// <summary>
        /// 给按钮添加点击按钮事件
        /// </summary>
        /// <param name="buttons"></param>
        public virtual void AddButtonListener(ref Button[] buttons)
        {
            foreach (Button button in buttons)//反射添加点击按钮事件
            {
                string FunctionName =  button.gameObject.name + "OnClick";
                Type t = this.GetType();
                MethodInfo func = t.GetMethod(FunctionName);
                //Debug.Log(func);
                button.onClick.AddListener(() =>
                {
                    func?.Invoke(this,null);
                });
            }
        }
    }
}
