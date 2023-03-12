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
	public partial class MapResLoad : ViewController, IController
    {     
		private ResLoader mResLoader = null;
        void Start()
		{
            
            mResLoader = ResLoader.Allocate();
            IGameSystem mSystem = this.GetSystem<IGameSystem>();
            if (!mSystem.MapInfoStatus)//û�ж�ȡ������
            {
                mResLoader.Add2Load<TextAsset>("mapdata_csv", "MapData", ((status,res) => 
                {
                    if (status)
                    {
                        Debug.Log("MapResLoad");
                        TextAsset MapDatatext = res.Asset as TextAsset;
                        string[] data = MapDatatext.text.Split("\r\n");
                        this.SendCommand(new LoadMapInfoEndCommand(data));
                    }
                }));
            }


            
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
