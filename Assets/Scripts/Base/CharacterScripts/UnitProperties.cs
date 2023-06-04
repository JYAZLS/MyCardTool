using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CardGameApp
{
    public class UnitProperties
    {
        public virtual string[] GetCommandBaseList(){
            string[] commandlist= {};
            return commandlist;
        }
        public virtual string[] GetTypeCommandList()
        {
            string[] commandlist= new string[]{};
            return commandlist;
        }
    }

    public class MasterProperties: UnitProperties
    {
        public override string[] GetCommandBaseList()
        {
            string[] commandlist= new string[]{
                "ChangeType",
                "Delete",
                "Cancel",
                "Wait"};
                return commandlist;
        }
        public override string[] GetTypeCommandList()
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
    
    public class SolderProperties: UnitProperties
    {
        public override string[] GetCommandBaseList()
        {
            string[] commandlist= new string[]{
                "Delete",
                "Cancel",
                "Wait"};
                return commandlist;
        }
    }
}
