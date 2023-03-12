using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;
using static CardGameApp.LoadCharacterInfoEndCommand;
using static CardGameApp.GameSystem;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace CardGameApp
{
	public partial class ResManager : ViewController, IController
    {
        private ResLoader mResLoader = null;
        private void Awake()
        {
            Debug.Log("ResKit.Init()");
            ResKit.Init();
        }
        void Start()
		{
            mResLoader = ResLoader.Allocate();
            //加载人物预制体
            this.RegisterEvent<StartLoadCharacterPrefabEvent>(e =>
            {
                //Debug.Log("StartLoadCharacterPrefab");
                IGameSystem gameSystem = this.GetSystem<IGameSystem>();
                foreach (var it in gameSystem.CharacterBaseInfo.Keys)
                {
                    //Debug.Log("it");
                    mResLoader.Add2Load<GameObject>("character",it,((status, res) =>
                    {
                        if (status)
                        {
                            GameObject game = new(it);
                            CharacterPool pool = game.AddComponent<CharacterPool>();
                            game.transform.SetParent(gameSystem.CharacterPoolMgr.transform);
                            pool.SetPrefab(res.Asset as GameObject);
                            pool.SetPrefabName(it);
                            gameSystem.CharacterPlayerPool.Add(it, pool);
                        }
                    }));
                }
                mResLoader.LoadAsync();
            });

        }
        private void OnDestroy()
        {
        }
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
       
    }
}
