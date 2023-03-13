using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;


// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace CardGameApp
{
    public enum EnumMapData
    {
        Name,
        WidthLen,
        HeightLen,
    }
    public enum EnumCharacterData
    {
        Name,
        Type,
        HP,
        CurrenHP,
        MoveRange
    }
	public partial class ResManager : UnitySingleton<ResManager>, IController
    {
        public ResLoader mResLoader = null;
        private void Awake()
        {
            Debug.Log("ResKit.Init()");
            ResKit.Init();
            //预制体创建
            GameObject GamePrefab = new GameObject("GamePrefab");
            CharacterPoolMgr = new GameObject("CharacterPoolMgr");
            CharacterPoolMgr.transform.SetParent(GamePrefab.transform);
            ButtonPoolMgr = new GameObject("ButtonPoolMgr");
            ButtonPool = ButtonPoolMgr.AddComponent<ButtonPool>();
            ButtonPoolMgr.transform.SetParent(GamePrefab.transform);
            PathPoolMgr = new GameObject("PathPoolMgr");
            PathPool = PathPoolMgr.AddComponent<PathPool>();
            PathPoolMgr.transform.SetParent(GamePrefab.transform);


            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        void Start()
		{
            mResLoader = ResLoader.Allocate();
            
            LoadBaseInfoRes();
            LoadPanelPrefab();
            LoadUIPrefab();
            mResLoader.LoadAsync();
        }
        private void OnDestroy()
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }

        //地图和人物基础数据添加
        public void LoadBaseInfoRes()
        {
            mResLoader.Add2Load<TextAsset>("mapdata_csv", "MapData", ((status,res) => 
            {
                if (status)
                {
                    //Debug.Log("MapResLoad");
                    TextAsset MapDatatext = res.Asset as TextAsset;
                    string[] data = MapDatatext.text.Split("\r\n");
                    for (int i = 1; i < data.Length; i++)
                    {
                        string[] Data = data[i].Split(',');
                        MapInfo mapInfo = new() { };
                        mapInfo.Name = Data[(int)EnumMapData.Name];
                        mapInfo.WidthLen = (int)float.Parse(Data[(int)EnumMapData.WidthLen]);
                        mapInfo.HeightLen = (int)float.Parse(Data[(int)EnumMapData.HeightLen]);
                        MapBaseInfo.Add(mapInfo.Name, mapInfo);
                    }
                }
            }));

            mResLoader.Add2Load<TextAsset>("characterdata_csv", "CharacterData", ((status, res) => 
            {
                if (status)
                {
                    TextAsset CharacterDatatext = res.Asset as TextAsset;
                    string[] data = CharacterDatatext.text.Split("\r\n");
                    for (int i = 1; i < data.Length; i++)
                    {
                        //Debug.Log(data[i]);
                        CharacterInfo characterInfo = new();
                        string[] Data = data[i].Split(',');
                        string Name = Data[(int)EnumCharacterData.Name];
                        characterInfo.Hp = (int)float.Parse(Data[(int)EnumCharacterData.HP]);
                        characterInfo.Type = Data[(int)EnumCharacterData.Type];
                        characterInfo.CurrentHP = (int)float.Parse(Data[(int)EnumCharacterData.CurrenHP]);
                        characterInfo.MoveRange = (int)float.Parse(Data[(int)EnumCharacterData.MoveRange]);
                        //加入数据
                        CharacterBaseInfo.Add(Name, characterInfo);
                    }
                    LoadCharacterPrefab();
                }
            }));
        }
        public void LoadPanelPrefab()
        {
            mResLoader.Add2Load<GameObject>("Panel", "MenuPanel", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("MenuPanel");
                    GameObject MenuPanel = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("MenuPanel", MenuPanel));
                }
            }));

            mResLoader.Add2Load<GameObject>("Panel", "SetGamePanel", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject SetGamePanel = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("SetGamePanel", SetGamePanel));
                }
            }));

            mResLoader.Add2Load<GameObject>("Panel", "ControlMenuPanel", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject ControlMenuPanel = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("ControlMenuPanel", ControlMenuPanel));
                }
            }));

            mResLoader.Add2Load<GameObject>("Panel", "CreateCharacterMenu", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject CreateCharacterMenu = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("CreateCharacterMenu", CreateCharacterMenu));
                }
            }));

            mResLoader.Add2Load<GameObject>("Panel", "EndMenu", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject EndMenu = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("EndMenu", EndMenu));
                }
            }));
            mResLoader.Add2Load<GameObject>("Panel", "CommandMenu", ((status, res) =>
            {
                if (status)
                {
                    //Debug.Log("SetGamePanel");
                    GameObject CommandMenu = res.Asset as GameObject;
                    this.SendCommand(new LoadUIResCommand("CommandMenu", CommandMenu));
                }
            }));
        }
        public void LoadCharacterPrefab()
        {
            foreach (var it in CharacterBaseInfo.Keys)
            {
                //Debug.Log("it");
                mResLoader.Add2Load<GameObject>("character",it,((status, res) =>
                {
                    if (status)
                    {
                        GameObject game = new(it);
                        CharacterPool pool = game.AddComponent<CharacterPool>();
                        game.transform.SetParent(CharacterPoolMgr.transform);
                        pool.SetPrefab(res.Asset as GameObject);
                        pool.SetPrefabName(it);
                        CharacterPlayerPool.Add(it, pool);
                    }
                }));
            }
        }
        public void LoadUIPrefab()
        {
             mResLoader.Add2Load<GameObject>("ui","Button", ((status,res) =>
            {
                if(status)
                {
                    GameObject buttonprefab = res.Asset as GameObject;
                    this.SendCommand(new LoadButtonPrefab(buttonprefab));
                }
            }));
            mResLoader.Add2Load<GameObject>("ui","Path", ((status,res) =>
            {
                if(status)
                {
                    GameObject Pathprefab = res.Asset as GameObject;
                    this.SendCommand(new LoadPathPrefab(Pathprefab));
                }
            }));
        }
    }
}
