using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public interface IUISystem : ISystem
    {
        public Dictionary<string, GameObject> UI_GameObjectRes { get; }
        public Dictionary<string, GameObject> UI_GameObject { get; }
        public UIManager UIManager { get;}
        public PanelManager PanelManager { get; set; }
        public void CreatePanel(string UI_Name,BasePanel panel);
        //public void PushPanel(string UI_Name, BasePanel panel);
        public void OpenUI(string UI);
        public void PopPanel();
        public void PanelClearAll();
        public string GetCurrentPanelName();
        public void PanelDestoryAll();
        public void DestroyUI(string ui);
    }
    /// <summary>
    /// UI系统，控制UI相关资源
    /// </summary>
    public class UISystem: AbstractSystem, IUISystem
    {
        public Dictionary<string, GameObject> UI_GameObjectRes { get; set; }
        public Dictionary<string, GameObject> UI_GameObject { get; set; } = new();
        Dictionary<string, BasePanel>  UI_Panel = new();
        public UIManager UIManager { get; set; }
        public PanelManager PanelManager { get; set; }

        protected override void OnInit()
        {
            UI_GameObjectRes = ResManager.Intance.UI_GameObjectRes;
            UIManager = new UIManager(UI_GameObjectRes, UI_GameObject);
            PanelManager = new PanelManager();
        }

        public void CreatePanel(string UI_Name,BasePanel panel)
        {
            panel.PanelObject =  UIManager.GetUI(UI_Name);
            //Debug.Log(panel);
            UI_Panel.Add(UI_Name,panel);
            PanelManager.Push(UI_Name,panel);
            panel.OnEnter();
            PanelManager.Pop();
        }

        // public void PushPanel(string UI_Name, BasePanel panel)
        // {
        //     panel.PanelObject = UIManager.GetUI(UI_Name);
        //     panel.PanelObject.SetActive(true);
        //     PanelManager.Push(UI_Name,panel);
        // }

        public void PopPanel()
        {
            PanelManager.Pop();
        }

        public void PanelClearAll()
        {
            PanelManager.ClearAll();
        }

        public void OpenUI(string UI)
        {
            UI_Panel[UI].PanelObject.SetActive(true);
            PanelManager.Push(UI,UI_Panel[UI]);
        }

        public string GetCurrentPanelName()
        {
            return PanelManager.GetCurrentPanelName();
        }
        public void PanelDestoryAll()
        {
            PanelManager.ClearAll();
            UI_Panel.Clear();
        }
        public void DestroyUI(string ui) 
        {
            UIManager.DestoryUI(ui);
        }
    }
}
