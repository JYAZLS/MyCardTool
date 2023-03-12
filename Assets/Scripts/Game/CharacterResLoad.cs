using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using static CardGameApp.LoadCharacterInfoEndCommand;

// 1.���ڲ˵� �༭����չ/Namespace Settings �����������ռ�
// 2.�����ռ���ĺ����ɴ���֮����Ҫ���߼������ļ����� Designer���������ռ��ֶ�����
namespace CardGameApp
{
	public partial class CharacterResLoad : ViewController, IController
    {     
		private ResLoader mResLoader = null;
        void Start()
		{
            mResLoader = ResLoader.Allocate();
            IGameSystem mSystem = this.GetSystem<IGameSystem>();
            if (!mSystem.CharacterInfoStatus)//û�ж�ȡ������
            {
                mResLoader.Add2Load<TextAsset>("characterdata_csv", "CharacterData", ((status, res) => 
                {
                    if (status)
                    {
                        TextAsset CharacterDatatext = res.Asset as TextAsset;
                        string[] data = CharacterDatatext.text.Split("\r\n");
                        this.SendCommand(new LoadCharacterInfoEndCommand(data));
                    }
                }));
                this.RegisterEvent<LoadCharacterInfoEndEvent>(e =>
                {
                    Destroy(gameObject);
                }).UnRegisterWhenGameObjectDestroyed(this.gameObject);
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
