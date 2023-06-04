using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class PanelManager
    {
        List<string> PanelList = new();
        private Stack<BasePanel> PanelStack;
        public PanelManager() {PanelStack = new Stack<BasePanel>();}
        private BasePanel panel;
        public void PushUI(string panelname,BasePanel NextPanel)
        { 
            if(PanelStack.Count>0)
            {
                panel = PanelStack.Peek();
                panel.OnPasue();//停止操作                 
            }
            PanelStack.Push(NextPanel);
            PanelList.Add(panelname);
        }

        public void PopUI()
        {
            if(PanelStack.Count>0)
            {
                PanelStack.Peek().OnExit();//退出最上层的Panel
                PanelList.Remove(PanelStack.Peek().PanelName);
                PanelStack.Pop();
            }
            if(PanelStack.Count>0)
            {
                PanelStack.Peek().OnResume();
            }
        }

        public void ClearAll()
        {
            while (PanelStack.Count > 0)
            {
                if (PanelStack.Count > 0)
                {
                    PanelStack.Peek().OnExit();//退出最上层的Panel
                    PanelList.Remove(PanelStack.Peek().PanelName);
                    PanelStack.Pop();
                }
                if (PanelStack.Count > 0)
                {
                    PanelStack.Peek().OnResume();
                }
            }
        }

        public string GetCurrentPanelName()
        {
            if(PanelStack.Count>0)
            {
                return PanelStack.Peek().PanelName;
            }
            return null;
        }

        public bool PopStrPanel(string UI)
        {
            bool flag = false;
            if(PanelList.Contains(UI))
            {
                Stack<BasePanel> tempPanel = new();
                while(!flag)
                {
                    if(PanelStack.Peek().PanelName == UI)
                    {
                        PopUI();
                        flag = true;
                    }
                    else
                    {
                        tempPanel.Push(PanelStack.Peek());
                    } 
                }
                while(tempPanel.Count>0)
                {
                    PushUI(tempPanel.Peek().PanelName,tempPanel.Peek());
                    tempPanel.Pop();
                }
            }
            return flag;
        }
    }
}
