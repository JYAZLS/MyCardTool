using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CardGameApp
{
    public enum TileType { GROUND, GRASS, WALL, TENX, HOUSE, MOUNTAIN, FOREST, REVIER, CASTLE }
    public interface IMapSystem : ISystem
    {
        public Tilemap Tilemaps { get; set; }
        public UnityEngine.Vector3 CursorVecter { get; set; }
        public List<GridInfo> TileInfo { get; set;}
        public GridInfo CurrentTile {get; set;}
        public void Updated();
        public void ShowMoveRange(Transform transform, int moveRange, string Militrary, int Team);
        public void ShowMovePath(Vector3 FromVec3,Vector3 ToVec3);
        public void ClearMoveRange();
        public void CheckColider();
    }
    public class MapSystem : AbstractSystem, IMapSystem
    {
        private float cameraMoveSpeed = 1f;
        private float borderWidth = 10f;
        //private float camerazoomSpeed = 0.5f;
        private float mapScaleMax = 2f;
        private float mapScaleMin = 1f;
        private IGameModel mModel;
        public Tilemap Tilemaps { get; set; }
        public UnityEngine.Vector3 CursorVecter { get; set; }
        public List<GridInfo> TileInfo { get; set; }= new(); 
        private UnityEngine.Vector3Int CellPosition;
        //private  List<SpriteRenderer> OpenList = new List<SpriteRenderer>();
        private  Dictionary<int,SpriteRenderer> OpenList = new();
        private  List<int> PathList = new();
        private PathPool PathPool;
        private GameObject PathPoolMgr; 
        public GridInfo CurrentTile {get; set;}
        int WidthLen {
            get{ return mModel.mapInfo.WidthLen; }
        }
        int HeightLen {
            get{ return mModel.mapInfo.HeightLen; }
        }
        protected override void OnInit()
        {
            mModel = this.GetModel<IGameModel>();
            PathPool = ResManager.Intance.PathPool;
            PathPoolMgr = ResManager.Intance.PathPoolMgr;
        }
        /// <summary>
        /// 控制缩放和地图移动
        /// </summary>
        public void Updated()
        {
            Vector3 MousePos = Input.mousePosition;
            float Wheel = Input.GetAxis("Mouse ScrollWheel");

            Camera camera = Camera.main;
            MousePos.z = 0;
            UnityEngine.Vector3 worldpos = Camera.main.ScreenToWorldPoint(MousePos);

            double minX, minY, maxX, maxY;

            minX = -WidthLen/2 * 0.48f - 0.48f;
            maxX = WidthLen / 2 * 0.48f + 0.48f;
            minY = -HeightLen / 2 * 0.48f - 0.48f;
            maxY = HeightLen / 2 * 0.48f + 0.48f;

            if ((MousePos.x <= borderWidth) && (worldpos.x >= minX))
            {
                camera.transform.Translate(-camera.transform.right * cameraMoveSpeed * Time.deltaTime, Space.World);
            }
            if ((MousePos.x >= Screen.width - borderWidth) && (worldpos.x <= maxX))
            {
                camera.transform.Translate(camera.transform.right * cameraMoveSpeed * Time.deltaTime, Space.World);
            }
            if ((MousePos.y <= borderWidth) && (worldpos.y >= minY))
            {
                camera.transform.Translate(-camera.transform.up * cameraMoveSpeed * Time.deltaTime, Space.World);
            }
            if ((MousePos.y >= Screen.height - borderWidth) && (worldpos.y <= maxY))
            {
                camera.transform.Translate(camera.transform.up * cameraMoveSpeed * Time.deltaTime, Space.World);
            }

            //控制缩放
            float NormalorthographicSize = camera.orthographicSize;
            //限制
            NormalorthographicSize -= Wheel;
            if (NormalorthographicSize >= mapScaleMax)
                NormalorthographicSize = mapScaleMax;
            else if (NormalorthographicSize <= mapScaleMin)
                NormalorthographicSize = mapScaleMin;
            camera.orthographicSize = NormalorthographicSize;

            //限制在地图内
            if (worldpos.x < minX + 0.48f)
            {
                worldpos.x = (float)(minX + 0.48f);
            }
            else if (worldpos.x > maxX - 0.48f)
            {
                worldpos.x = (float)(maxX - 0.48f);
            }
            if (worldpos.y < minY + 0.48f)
            {
                worldpos.y = (float)(minY + 0.48f);
            }
            else if (worldpos.y > maxY - 0.48f)
            {
                worldpos.y = (float)(maxY - 0.48f);
            }
            CellPosition = World2Cell(worldpos);
            CursorVecter = Tilemaps.CellToWorld(CellPosition);
            Vector3Int tilevector = World2Tile(worldpos);
            int index = tilevector.y * WidthLen + tilevector.x;
            CurrentTile = TileInfo[index];
            //Debug.Log(index);
        }
        public int ConvertWorldToTile(float value)
        {
            if (value > 0)
            {
                return (int)(value / 0.48f);
            }
            else
            {
                return (int)(value / 0.48f) - 1;
            }

        }

        public Vector3 Tile2World(Vector3Int _vector)
        {
            Vector3 vector3 = Vector3.zero;
            vector3.x = (_vector.x - WidthLen/2) * 0.48f + 0.24f;
            vector3.y = (HeightLen/2 - _vector.y) * 0.48f - 0.24f;
            return vector3;
        }
        public Vector3Int Cell2Tile(Vector3Int _vector)
        {
            Vector3Int vector3 = Vector3Int.zero;
            vector3.x = _vector.x  + WidthLen/2;
            vector3.y = -_vector.y - 1 + HeightLen/2;
            return vector3;
        }
        public Vector3 Cell2World(Vector3Int _vector)
        {
            Vector3 vector3 = Vector3.zero;
            vector3.x =  _vector.x  * 0.48f + 0.24f;
            vector3.y =  _vector.y  * 0.48f + 0.24f;
            return vector3;
        }
        public Vector3Int Tile2Cell(Vector3Int _vector)
        {
            Vector3Int vector3 = Vector3Int.zero;
            vector3.x = _vector.x - WidthLen/2;
            vector3.y = -_vector.y + HeightLen/2 - 1;
            return vector3;
        }
        public Vector3Int World2Cell(Vector3 _vector)
        {
            Vector3Int vector3 = Vector3Int.zero;
            vector3.x = (_vector.x > 0)?(int)(_vector.x / 0.48f):((int)(_vector.x / 0.48f) - 1);
            vector3.y = (_vector.y > 0)?(int)(_vector.y / 0.48f):((int)(_vector.y / 0.48f) - 1);
            return vector3;
        }
        public Vector3Int World2Tile(Vector3 _vector)
        {
            Vector3Int vector3 = Vector3Int.zero;
            vector3 = World2Cell(_vector);
            vector3 = Cell2Tile(vector3);
            return vector3;
        }
        /// <summary>
        /// 显示移动范围
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="moveRange"></param>
        /// <param name="Militrary"></param>
        /// <param name="Team"></param>
        public void ShowMoveRange(Transform transform, int moveRange, string Militrary, int Team)
        {
            PathPool PathPool = ResManager.Intance.PathPool;
            //Debug.Log("Cell:"+CellPosition);
            Vector3Int TileVec3 = Cell2Tile(CellPosition);
            int index = TileVec3.y * WidthLen + TileVec3.x;
            SpriteRenderer render =  PathPool.Get();
            render.color = Color.green;
            render.transform.position = new Vector3(transform.position.x,transform.position.y,0);
            OpenList.Add(index,render);//添加本身地图块
            //Debug.Log(moveRange);
            //Debug.Log("Tile:"+TileVec3);
            findCanWalkTileUp(TileVec3, moveRange, Militrary, Team);
            findCanWalkTileRight(TileVec3, moveRange, Militrary, Team);
            findCanWalkTileLeft(TileVec3, moveRange, Militrary, Team);
            findCanWalkTileDown(TileVec3, moveRange, Militrary, Team);
        }
        
        public void CheckColider()
        {
            for(int i = 0;i<HeightLen;i++)
            {
                for(int j = 0; j < WidthLen; j++)
                {
                    Vector3Int Cell = Tile2Cell(new Vector3Int(j, i, 0));
                    int index = i * WidthLen + j;
                    Vector3 Celltrans = Tilemaps.CellToWorld(Cell);
                    Celltrans.x += 0.24f;
                    Celltrans.y += 0.24f;
                    Collider2D collider = Physics2D.OverlapBox(Celltrans, new Vector2(0.1f, 0.1f), 0, 1 << 3);
                    if(TileInfo[index].GridType == (int)TileType.WALL)
                    {
                        continue;
                    }
                    if (collider != null)
                    {
                        TileInfo[index].ColiderBox = true;
                    }
                    else
                    {
                        TileInfo[index].ColiderBox = false;
                    }
                }
            }
        }
        private void findCanWalkTileUp(Vector3Int tilevector , int moveRange, string Militrary, int Team)
        {
            Vector3Int currentvector = tilevector;
            if (moveRange == 0)
            {
                return;
            }
            currentvector.y = currentvector.y - 1;
            if (currentvector.y < -1)
            {
                return;
            }
            int index = currentvector.y * WidthLen + currentvector.x;
            //Debug.Log(index+ " " + TileInfo[index].ColiderBox + " "+ TileInfo[index].GridType);
            if (TileInfo[index].ColiderBox == false)//没有阻挡且OpenList没有包含在内
            {
                if(!OpenList.ContainsKey(index))
                {
                    SpriteRenderer render =  PathPool.Get();
                    render.gameObject.transform.SetParent(PathPoolMgr.transform);
                    Vector3 vec = Tile2World(currentvector);
                    render.color = Color.green;
                    render.transform.position = new Vector3(vec.x,vec.y,0);
                    OpenList.Add(index,render);
                }  
            }
            else{
                return;
            }
           

            int temMoveRange = 0;
            //计算路径消耗
            temMoveRange = moveRange - 1;
            findCanWalkTileUp(currentvector, temMoveRange, Militrary, Team);
            findCanWalkTileRight(currentvector, temMoveRange, Militrary, Team);
            findCanWalkTileLeft(currentvector, temMoveRange, Militrary, Team);
        }
        private void findCanWalkTileDown(Vector3Int tilevector, int moveRange, string Militrary, int Team)
        {
            Vector3Int currentvector = tilevector;
            if (moveRange == 0)
            {
                return;
            }
            currentvector.y = currentvector.y + 1;
            if (currentvector.y >= HeightLen)
            {
                return;
            }

            int index = currentvector.y * WidthLen + currentvector.x;
            //Debug.Log("down:"+index+ " " + TileInfo[index].ColiderBox + " "+ TileInfo[index].GridType);
            if (TileInfo[index].ColiderBox == false)//没有阻挡且OpenList没有包含在内
            {
                if(!OpenList.ContainsKey(index))
                {
                    SpriteRenderer render =  PathPool.Get();
                    render.gameObject.transform.SetParent(PathPoolMgr.transform);
                    Vector3 vec = Tile2World(currentvector);
                    render.color = Color.green;
                    render.transform.position = new Vector3(vec.x,vec.y,0);
                    OpenList.Add(index,render);
                }             
            }
            else
            {
                return;
            }
            int temMoveRange = 0;
            //计算路径消耗
            temMoveRange = moveRange - 1;
            findCanWalkTileDown(currentvector, temMoveRange, Militrary, Team);
            findCanWalkTileRight(currentvector, temMoveRange, Militrary, Team);
            findCanWalkTileLeft(currentvector, temMoveRange, Militrary, Team);
        }
        private void findCanWalkTileRight(Vector3Int tilevector, int moveRange, string Militrary, int Team)
        {
            Vector3Int currentvector = tilevector;
            if (moveRange == 0)
            {
                return;
            }
            currentvector.x = currentvector.x + 1;
            if (currentvector.x >= WidthLen)
            {
                return;
            }
            int index = currentvector.y * WidthLen + currentvector.x;
            //Debug.Log(index+ " " + TileInfo[index].ColiderBox + " "+ TileInfo[index].GridType);
            if (TileInfo[index].ColiderBox == false)//没有阻挡且OpenList没有包含在内
            {
                if(!OpenList.ContainsKey(index))
                {
                    SpriteRenderer render =  PathPool.Get();
                    render.gameObject.transform.SetParent(PathPoolMgr.transform);
                    Vector3 vec = Tile2World(currentvector);
                    render.color = Color.green;
                    render.transform.position = new Vector3(vec.x,vec.y,0);
                    OpenList.Add(index,render);
                }  
                
            }
            else
            {
                return;
            }

            int temMoveRange = 0;
            //计算路径消耗
            temMoveRange = moveRange - 1;
            findCanWalkTileRight(currentvector, temMoveRange, Militrary, Team);
            findCanWalkTileUp(currentvector, temMoveRange, Militrary, Team);
            findCanWalkTileDown(currentvector, temMoveRange, Militrary, Team);
        }
        private void findCanWalkTileLeft(Vector3Int tilevector, int moveRange, string Militrary, int Team)
        {
            Vector3Int currentvector = tilevector;
            if (moveRange == 0)
            {
                return;
            }
            currentvector.x = currentvector.x - 1;
            if (currentvector.x < 0)
            {
                return;
            }
            int index = currentvector.y * WidthLen + currentvector.x;
            //Debug.Log(index+ " " + TileInfo[index].ColiderBox + " "+ TileInfo[index].GridType);
            if (TileInfo[index].ColiderBox == false)//没有阻挡且OpenList没有包含在内
            {
                if(!OpenList.ContainsKey(index))
                {
                    SpriteRenderer render =  PathPool.Get();
                    render.gameObject.transform.SetParent(PathPoolMgr.transform);
                    Vector3 vec = Tile2World(currentvector);
                    render.color = Color.green;
                    render.transform.position = new Vector3(vec.x,vec.y,0);
                    OpenList.Add(index,render);
                }  
                
            }
            else
            {
                return;
            }
            int temMoveRange = 0;
            //计算路径消耗
            temMoveRange = moveRange - 1;
            findCanWalkTileUp(currentvector, temMoveRange, Militrary, Team); 
            findCanWalkTileLeft(currentvector, temMoveRange, Militrary, Team);
            findCanWalkTileDown(currentvector, temMoveRange, Militrary, Team);
        }

        public void ShowMovePath(Vector3 FromVec3,Vector3 ToVec3)
        {
            foreach (var it in OpenList.Values)
            {
                it.color = Color.green;
            }
            PathList.Clear();
            //父节点
            Vector3Int FromTile = World2Tile(FromVec3);
            //Debug.Log("FromTile:"+ FromTile);
            int Fromindex = FromTile.y * WidthLen + FromTile.x;
            //起始点
            Vector3Int StartTile = new Vector3Int(FromTile.x,FromTile.y);
            int Startindex = Fromindex;
            //结果点
            Vector3 actualtile = new Vector3(ToVec3.x + 0.24f, ToVec3.y + 0.24f,0); //偏移
            Vector3Int ToTile = World2Tile(actualtile);
            //Debug.Log("ToVec3:"+ ToVec3);
            //Debug.Log("ToTile:"+ ToTile);
            int Toindex = ToTile.y * WidthLen + ToTile.x;
            //过程点
            Vector3Int TemTile = new Vector3Int(FromTile.x,FromTile.y);
            int Temindex = Fromindex;

            HashSet<int> openlist= new HashSet<int>();
            HashSet<int> closelist = new HashSet<int>();
            Dictionary<int,int> FnValuePairs= new Dictionary<int,int>();
            Dictionary<int, int> HnValuePairs = new Dictionary<int, int>();
            Dictionary<int, int> OpenHnValuePairs = new Dictionary<int, int>();
            if (!OpenList.ContainsKey(Toindex))
            {
                return;
            }
            else if (Toindex == Startindex)
            {
                OpenList[Startindex].color = Color.blue;
            }
            else
            {
                bool CalFlag = false;
                openlist.Add(Startindex);
                PathList.Add(Startindex);
                //int test = 2;
                //Debug.Log("Start");
                while (!CalFlag)
                {
                    
                    // HnValuePairs.Clear();
                    // FnValuePairs.Clear();
                    int CurrentHn = Mathf.Abs(ToTile.y - FromTile.y) + Mathf.Abs(ToTile.x - FromTile.x);

                    //4个方向进行判断
                    // TemTile.x = FromTile.x - 1;
                    // TemTile.y = FromTile.y;
                    TemTile.x = FromTile.x-1;
                    TemTile.y = FromTile.y;
                    
                    Temindex = Fromindex - 1;
                    //Debug.Log(Temindex);
                    if (TemTile.x  > 0 && !closelist.Contains(Temindex) && !TileInfo[Temindex].ColiderBox && OpenList.ContainsKey(Temindex))
                    {
                        int Hn = Mathf.Abs(ToTile.y - TemTile.y) + Mathf.Abs(ToTile.x - TemTile.x);
                        int Gn = Mathf.Abs(TemTile.y - StartTile.y) + Mathf.Abs(TemTile.x - StartTile.x);

                        //Debug.Log(Temindex+" "+Hn+ " "+CurrentHn);
                        if (!openlist.Contains(Temindex))
                        {
                            openlist.Add(Temindex);
                            HnValuePairs.Add(Temindex, Hn);
                            FnValuePairs.Add(Temindex, Hn + Gn);
                        }
                    }

                    TemTile.x = FromTile.x + 1;
                    TemTile.y = FromTile.y;
                    Temindex = Fromindex + 1;
                    if (TemTile.x < WidthLen && !closelist.Contains(Temindex) && !TileInfo[Temindex].ColiderBox && OpenList.ContainsKey(Temindex))
                    {
                        int Hn = Mathf.Abs(ToTile.y - TemTile.y) + Mathf.Abs(ToTile.x - TemTile.x);
                        int Gn = Mathf.Abs(TemTile.y - StartTile.y) + Mathf.Abs(TemTile.x - StartTile.x);

                        
                        //Debug.Log(Temindex+" "+Hn+ " "+CurrentHn);
                        if (!openlist.Contains(Temindex))
                        {
                            openlist.Add(Temindex);
                            HnValuePairs.Add(Temindex, Hn);
                            FnValuePairs.Add(Temindex, Hn + Gn);
                        }
                    }

                    TemTile.x = FromTile.x;
                    TemTile.y = FromTile.y + 1;
                    Temindex = Fromindex + WidthLen;
                    if (TemTile.y < HeightLen && !closelist.Contains(Temindex) && !TileInfo[Temindex].ColiderBox && OpenList.ContainsKey(Temindex))
                    {
                        int Hn = Mathf.Abs(ToTile.y - TemTile.y) + Mathf.Abs(ToTile.x - TemTile.x);
                        int Gn = Mathf.Abs(TemTile.y - StartTile.y) + Mathf.Abs(TemTile.x - StartTile.x);

                        //Debug.Log(Temindex+" "+Hn+ " "+CurrentHn);
                        if (!openlist.Contains(Temindex))
                        {
                            openlist.Add(Temindex);
                            HnValuePairs.Add(Temindex, Hn);
                            FnValuePairs.Add(Temindex, Hn + Gn);
                        }
                    }

                    TemTile.x = FromTile.x;
                    TemTile.y = FromTile.y - 1;
                    Temindex = Fromindex - WidthLen;
                    if (TemTile.y > 0 && !closelist.Contains(Temindex) && !TileInfo[Temindex].ColiderBox && OpenList.ContainsKey(Temindex))
                    {
                        int Hn = Mathf.Abs(ToTile.y - TemTile.y) + Mathf.Abs(ToTile.x - TemTile.x);
                        int Gn = Mathf.Abs(TemTile.y - StartTile.y) + Mathf.Abs(TemTile.x - StartTile.x);

                        //Debug.Log(Temindex+" "+Hn+ " "+CurrentHn);
                        if (!openlist.Contains(Temindex))
                        {
                            openlist.Add(Temindex);
                            HnValuePairs.Add(Temindex, Hn);
                            FnValuePairs.Add(Temindex, Hn + Gn);
                        }
                    }

                    openlist.Remove(Fromindex);//移除父节点
                    HnValuePairs.Remove(Fromindex);
                    FnValuePairs.Remove(Fromindex);
                    closelist.Add(Fromindex);

                    int MinIndex = -1;
                    int MinHn = -1;
                    //Debug.Log("HnValuePairs:");
                    foreach (var it in HnValuePairs)//找到下个方向中最小估计值
                    {
                        
                        //Debug.Log(it.Key + " "+ it.Value);
                        if (MinIndex == -1)
                        {
                            MinIndex = it.Key;
                            MinHn = it.Value;
                        }
                        else
                        {
                            if (it.Value <= MinHn)
                            {
                                MinIndex = it.Key;
                                MinHn = it.Value;
                            }
                        }
                    }
                    //Debug.Log("Min:" + MinIndex + " " + MinHn);
                    for (int i = 0; i < openlist.Count; i++)//与开列表中的进行判断，是否有更优的解
                    {
                        int col = openlist.ElementAt(i) % WidthLen;
                        int row = openlist.ElementAt(i) / WidthLen;
                        int Hn = Mathf.Abs(ToTile.y - row) + Mathf.Abs(ToTile.x - col);
                        if (Hn <= MinHn)
                        {
                            MinIndex = openlist.ElementAt(i);
                            openlist.Remove(MinIndex);
                            HnValuePairs.Remove(MinIndex);
                            FnValuePairs.Remove(MinIndex);
                            i--;
                            closelist.Add(MinIndex);
                            break;
                        }
                    }

                    int diff = Mathf.Abs(MinIndex - PathList.Last());//看看新节点是否衔接上一个，如果不是删除上一个路径，添加新路径
                    if ((diff == 1) || (diff == 20))
                    {
                        PathList.Add(MinIndex);
                    }
                    else
                    {
                        PathList.Remove(PathList.Last());
                        PathList.Add(MinIndex);
                    }
                    
                    Fromindex = MinIndex;
                    FromTile.x = MinIndex % WidthLen;
                    FromTile.y = MinIndex / WidthLen;
                    if (MinIndex == Toindex)
                    {
                        //Debug.Log("Cal is ok");
                        foreach (var it in PathList)
                        {
                            OpenList[it].color = Color.blue;
                        }
                        CalFlag = true;
                    }
                }
            }
        }
        public void ClearMoveRange()
        {
            foreach(var it in OpenList.Values)
            {
                PathPool.Release(it);          
            }
            PathList.Clear();
            OpenList.Clear();
            
        }
    }   


}
