using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class Solder : CharacterBase,ICharacter
    {
        GameObject ICharacter.mGameObject => this.gameObject;
        CharacterAttr ICharacter.CharacterAttr => CharacterAttr;
        Military ICharacter.Military => Military;
        public void SetTeam(int Team)
        {
            this.Team = Team;
        }

        public string[] GetCommandBaseList()
        {
            string[] commandlist = null;
            commandlist= new string[]{
                "Delete",
                "Cancel",
                "Wait"};
                return commandlist;
        }
    }
}

