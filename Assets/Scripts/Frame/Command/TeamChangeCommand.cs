using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace CardGameApp
{
    public class TeamChangeCommand : AbstractCommand
    {
        // Start is called before the first frame update
        protected override void OnExecute()
        {
            IBattleSystem battleSystem = this.GetSystem<IBattleSystem>();
            battleSystem.TeamNum.Value++;
            if(battleSystem.TeamNum.Value >= battleSystem.TeamTotalNum)
            {
                battleSystem.TeamNum.Value = 0;
            }            
        }
    }
}