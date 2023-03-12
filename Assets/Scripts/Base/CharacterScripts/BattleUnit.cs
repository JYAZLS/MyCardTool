using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class BattleUnit : MonoBehaviour,ICharacter
    {
        public Solder CharacterInfo = new Solder();

        public int Team => CharacterInfo.Team;

        GameObject ICharacter.mGameObject => this.gameObject;

        CharacterAttr ICharacter.CharacterAttr => CharacterInfo.characterAttr;

        Military ICharacter.Military => CharacterInfo.military;

        // Start is called before the first frame update
        public void SetTeam(int Team)
        {
            CharacterInfo.Team = Team;
        }

        public string[] GetCommandBaseList()
        {
            string[] commandlist = null;
            if(CharacterInfo.characterAttr.Name == CharacterInfo.military.MilitaryName)
            {
                commandlist= new string[]{
                    "Delete",
                    "Cancel",
                    "Wait"};
            }
            else
            {
                commandlist= new string[]{
                    "ChangeType",
                    "Delete",
                    "Cancel",
                    "Wait"};
            }
            return commandlist;
        }
    }
}

