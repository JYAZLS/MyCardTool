using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class GameFrame : Architecture<GameFrame>
    {
        protected override void Init()
        {
            //RegisterUtility<IStorage>(new MySaveStorage());

            RegisterModel<IGameModel>(new GameModel());

            RegisterSystem<IMapSystem>(new MapSystem());
            RegisterSystem<IUISystem>(new UISystem());
            RegisterSystem<IBattleSystem>(new BattleSystem());


        }
    }

}
