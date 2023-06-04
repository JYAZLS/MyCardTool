// Generate Id:cc75cb03-ad93-4ca9-91ab-b5c5f9611bd7
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1.���ڲ˵� �༭����չ/Namespace Settings �����������ռ�
// 2.�����ռ���ĺ����ɴ���֮����Ҫ���߼������ļ����� Designer���������ռ��ֶ�����
namespace CardGameApp
{
	public partial class ResManager
	{
		public Dictionary<string, GameObject> UI_GameObjectRes { get; set;} = new();
		public Dictionary<string, CharacterInfo> CharacterBaseInfo { get; set;} = new();//��������������
        public Dictionary<string, MapInfo> MapBaseInfo { get; set;} = new();//��ŵ�ͼ��������
        public Dictionary<string, CharacterPool> CharacterPlayerPool { get; set; } = new();
        public GameObject CharacterPoolMgr { get; set;}
        public ButtonPool ButtonPool { get; set;}
        public GameObject ButtonPoolMgr {get; set;}
        public PathPool   PathPool{get; set;}
        public GameObject PathPoolMgr { get; set;}
        public GameObject Cursor { get; set;}
	}
}
