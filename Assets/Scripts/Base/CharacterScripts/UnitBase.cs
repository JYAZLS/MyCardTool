using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace CardGameApp
{

    public class UnitBase : MonoBehaviour
    {
        public int id;
        public int Team;
        public UnitAttr  Attr;
        public UnitMilitary Military;
        public UnitProperties properties;
        // Start is called before the first frame update
        void Awake()
        {
            Attr = new();
            Military = new();
        }
        public void SetTeam(int team)
        {
            Team = team;
        }
    }

    [System.Serializable]
    public struct UnitAttr
    {     
        public string Name;
        public string baseName;
        public int Hp;
        public int CurrentHp;
        public int MoveRange;
        public int AttackRange;       
    }

    /// <summary>
    /// ±¯÷÷…Ë∂®
    /// </summary>
    [System.Serializable]
    public struct UnitMilitary
    {
        // Start is called before the first frame update
        public string MilitaryName;
        public int MoveRange;
        public int AttacKRange;
    }

}
