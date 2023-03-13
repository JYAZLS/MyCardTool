using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace CardGameApp
{
    public interface ICharacter
    {
        public int Team {get;}
        public GameObject mGameObject { get; }
        public CharacterAttr CharacterAttr{ get; }
        public Military Military { get;  }
        public void SetTeam(int Team);
        public string[] GetCommandBaseList();
    }
    public class CharacterBase: MonoBehaviour
    {
        public int Team {get; set;}
        public GameObject mGameObject { get; set; }
        public CharacterAttr CharacterAttr{  get; set; } = new();
        public Military Military { get; set; }= new();

    }
}

