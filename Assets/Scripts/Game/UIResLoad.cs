using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using static CardGameApp.LoadMapInfoEndCommand;

// 1.���ڲ˵� �༭����չ/Namespace Settings �����������ռ�
// 2.�����ռ���ĺ����ɴ���֮����Ҫ���߼������ļ����� Designer���������ռ��ֶ�����
namespace CardGameApp
{
	public partial class UIResLoad : ViewController, IController
    {
        private ResLoader mResLoader = null;
        public struct UIResLoaderEndEvent{};
        void Start()
		{
            mResLoader = ResLoader.Allocate();
            mResLoader.Add2Load<GameObject>("Panel", "MenuPanel", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("MenuPanel");
                    GameObject MenuPanel = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("MenuPanel", MenuPanel));
                }
            }));

            mResLoader.Add2Load<GameObject>("Panel", "SetGamePanel", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject SetGamePanel = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("SetGamePanel", SetGamePanel));
                }
            }));

            mResLoader.Add2Load<GameObject>("Panel", "ControlMenuPanel", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject ControlMenuPanel = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("ControlMenuPanel", ControlMenuPanel));
                }
            }));

            mResLoader.Add2Load<GameObject>("Panel", "CreateCharacterMenu", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject CreateCharacterMenu = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("CreateCharacterMenu", CreateCharacterMenu));
                }
            }));

            mResLoader.Add2Load<GameObject>("Panel", "EndMenu", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject EndMenu = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("EndMenu", EndMenu));
                }
            }));
            mResLoader.Add2Load<GameObject>("Panel", "CommandMenu", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject CommandMenu = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("CommandMenu", CommandMenu));
                }
            }));
            mResLoader.Add2Load<GameObject>("ui","Button", ((status,res) =>
            {
                if(status)
                {
                    GameObject buttonprefab = res.Asset as GameObject;
                    this.SendCommand(new LoadButtonPrefab(buttonprefab));
                }
            }));
            mResLoader.Add2Load<GameObject>("ui","Path", ((status,res) =>
            {
                if(status)
                {
                    GameObject Pathprefab = res.Asset as GameObject;
                    this.SendCommand(new LoadPathPrefab(Pathprefab));
                }
            }));

            mResLoader.LoadAsync();
        }
        private void OnDestroy()
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
       
    }
}
