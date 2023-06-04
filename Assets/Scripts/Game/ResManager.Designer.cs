// Generate Id:cc75cb03-ad93-4ca9-91ab-b5c5f9611bd7
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace CardGameApp
{
	public partial class ResManager
	{
		public Dictionary<string, GameObject> UI_GameObjectRes { get; set;} = new();
		public Dictionary<string, CharacterInfo> CharacterBaseInfo { get; set;} = new();//存放人物基础数据
        public Dictionary<string, MapInfo> MapBaseInfo { get; set;} = new();//存放地图基础数据
        public Dictionary<string, CharacterPool> CharacterPlayerPool { get; set; } = new();
        public GameObject CharacterPoolMgr { get; set;}
        public ButtonPool ButtonPool { get; set;}
        public GameObject ButtonPoolMgr {get; set;}
        public PathPool   PathPool{get; set;}
        public GameObject PathPoolMgr { get; set;}
        public GameObject Cursor { get; set;}
	}
}
