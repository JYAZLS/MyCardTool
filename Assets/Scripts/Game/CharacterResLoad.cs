using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using static CardGameApp.LoadCharacterInfoEndCommand;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace CardGameApp
{
	public partial class CharacterResLoad : ViewController, IController
    {     
		private ResLoader mResLoader = null;
        void Start()
		{
            mResLoader = ResLoader.Allocate();
            IGameSystem mSystem = this.GetSystem<IGameSystem>();
            if (!mSystem.CharacterInfoStatus)//没有读取过数据
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
