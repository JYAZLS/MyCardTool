using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class PanelManager
    {
        private Stack<BasePanel> PanelStack;
        private Stack<string> PanelName;
        public PanelManager() {PanelStack = new Stack<BasePanel>(); PanelName = new Stack<string>();}
        private BasePanel panel;
        public void Push(string panelname,BasePanel NextPanel)
        { 
            if(PanelStack.Count>0)
            {
                panel = PanelStack.Peek();
                panel.OnPasue();//停止操作                 
            }
            PanelStack.Push(NextPanel);
            PanelName.Push(panelname);
        }

        public void Pop()
        {
            if(PanelStack.Count>0)
            {
                PanelStack.Peek().OnExit();//退出最上层的Panel
                PanelStack.Pop();
                PanelName.Peek();
                PanelName.Pop();
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
                    PanelStack.Pop();
                    PanelName.Peek();
                    PanelName.Pop();
                }
                if (PanelStack.Count > 0)
                {
                    PanelStack.Peek().OnResume();
                }
            }
        }

        public string GetCurrentPanelName()
        {
            if(PanelName.Count>0)
            {
                return PanelName.Peek();
            }
            return null;
        }
    }
}
