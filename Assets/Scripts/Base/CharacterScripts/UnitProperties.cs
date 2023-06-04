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
                "шる條", 
                "ш祭條", 
                "輪怹條", 
                "僮條",   
                "殢條",   
                "殢る條",  
                "僮る條", 
                "笭祭條", 
                "笭る條", 
                "聒崞芛醴",
                "覺尪"
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
