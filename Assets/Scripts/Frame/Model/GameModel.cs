using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CardGameApp
{
    public interface IGameModel : IModel
    {
        public MapInfo mapInfo { get; }
        public PlayerInfo playerInfo { get; }
    }
    public class GameModel : AbstractModel, IGameModel
    {
        public MapInfo mapInfo { get; } = new MapInfo();
        public PlayerInfo playerInfo { get; } = new PlayerInfo();

        protected override void OnInit()
        {

        }
    }

    public class MapInfo 
    {
        public string Name;
        public int WidthLen;
        public int HeightLen;
    }

    public class PlayerInfo
    {
        //��������
        public int PlayerTeamNumber;
        //�����Լ���Ӧ�Ľ�ɫ
    }
    public struct CharacterInfo
    {
        public int Hp;
        public string Type;
        public int CurrentHP;
        public int MoveRange;
    }
}

