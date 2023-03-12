using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class LoadUIResCommand : AbstractCommand
    {
        string UIName;
        GameObject gameobject = null;

        public LoadUIResCommand(string Name,GameObject game)
        {
            gameobject = game;
            UIName = Name;
        }
        protected override void OnExecute()
        {
            IUISystem msystem = this.GetSystem<IUISystem>();
            
            if (!msystem.UI_GameObjectRes.ContainsKey(UIName))//如果不存在该资源，则添加资源
            {
                msystem.UI_GameObjectRes.Add(UIName, gameobject);
            }
        }
    }

    public class LoadButtonPrefab : AbstractCommand
    {
        GameObject gameObject = null;

        public LoadButtonPrefab(GameObject game)
        {
            gameObject = game;
        }
        protected override void OnExecute()
        {
            IGameSystem gameSystem = this.GetSystem<IGameSystem>();
            gameSystem.ButtonPool.SetPrefab(gameObject);
        }
    }

    public class LoadPathPrefab : AbstractCommand
    {
        GameObject gameObject = null;

        public LoadPathPrefab(GameObject game)
        {
            gameObject = game;
        }
        protected override void OnExecute()
        {
            IGameSystem gameSystem = this.GetSystem<IGameSystem>();
            gameSystem.PathPool.SetPrefab(gameObject);
        }
    }
}

