using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class Master : CharacterBase,ICharacter
    {
        // Start is called before the first frame update
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
                "ChangeType",
                "Delete",
                "Cancel",
                "Wait"};
                return commandlist;
        }
    }
}

