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
        public GridInfo CurrentTile { get; set;}
        public GameObject Cursor { get;}
        int WidthLen {get;}
        int HeightLen {get;}
        public void Updated();
        //public void ShowMovePath(Vector3 FromVec3,Vector3 ToVec3);
        public void ClearMoveRange();
        public void CheckColider();
        public void getPathTransform(ref List<Vector3> path);
        //public bool isInOpenList(int index);
        public void AddCursor(GameObject _Cursor);
        public void FillColor(List<Vector3> path,Color color);
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
        //private  Dictionary<int,SpriteRenderer> OpenList = new();
        private  List<int> PathList = new();
        private PathPool PathPool;
        private GameObject PathPoolMgr; 
        public GridInfo CurrentTile {get; set;}
        public GameObject Cursor { get; set;} = null;
        private List<SpriteRenderer>  PathObjects = new();
        public int WidthLen {
            get{ return mModel.mapInfo.WidthLen; }
        }
        public int HeightLen {
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
        
        public void ClearMoveRange()
        {
            foreach(var it in PathObjects)
            {
                PathPool.Release(it);          
            }
            PathObjects.Clear();
            
        }
        /// <summary>
        /// 获取路径各个节点
        /// </summary>
        /// <param name="path"></param>
        public void getPathTransform(ref List<Vector3> path)
        {
            foreach (var it in PathList)
            {
                Vector3Int Tile = Vector3Int.zero;
                Tile.x = it % WidthLen;
                Tile.y = it / WidthLen;
                
                path.Add(Tile2World(Tile));
            }
        }

        public void AddCursor(GameObject _Cursor)
        {
            if(Cursor == null)
            {
                Cursor = _Cursor;
            }
            else
            {
                Cursor.DestroySelf();
            }
        }

        public void FillColor(List<Vector3> path,Color color)
        {
            foreach(Vector3 it in path)
            {
                SpriteRenderer render =  PathPool.Get();
                render.gameObject.transform.SetParent(PathPoolMgr.transform);
                render.color = color;
                render.transform.position = it;
                PathObjects.Add(render);      
            }  
        }
        
    }   


}
