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
        public Dictionary<string,int> status {get; set;}
        public void SetTeam(int Team);
        public string[] GetCommandBaseList();
        public string[] GetTypeCommandList();
    }
    public class CharacterBase: MonoBehaviour
    {
        public int Team {get; set;}
        public GameObject mGameObject { get; set; }
        public CharacterAttr CharacterAttr{  get; set; } = new();
        public Military Military { get; set; }= new();
        public Dictionary<string,int> status {get; set;}= new();

        public virtual string[] GetCommandBaseList()
        {
            string[] commandlist= new string[]{
                "ChangeType",
                "Delete",
                "Cancel",
                "Wait"};
                return commandlist;
        }

        public virtual string[] GetTypeCommandList()
        {
            string[] commandlist= new string[]{
                "�����", 
                "�Ჽ��", 
                "������", 
                "����",   
                "���",   
                "�����",  
                "�����", 
                "�ز���", 
                "�����", 
                "����ͷĿ",
                "ıʿ"
            };
            return commandlist;
        }

    }
}

