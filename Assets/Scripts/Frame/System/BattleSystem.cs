using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace CardGameApp
{
    public interface IBattleSystem : ISystem
    {
    }
    public class BattleSystem : AbstractSystem, IBattleSystem
    {       
        protected override void OnInit()
        {
        }

    }
}
