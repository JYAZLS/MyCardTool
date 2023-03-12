using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;
using static CardGameApp.LoadCharacterInfoEndCommand;
using static CardGameApp.GameSystem;

// 1.���ڲ˵� �༭����չ/Namespace Settings �����������ռ�
// 2.�����ռ���ĺ����ɴ���֮����Ҫ���߼������ļ����� Designer���������ռ��ֶ�����
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
            //��������Ԥ����
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
