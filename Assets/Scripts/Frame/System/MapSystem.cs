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
        public MapManager MapMgr {get; set;}

        public int WidthLen {get{return MapMgr?MapMgr.WidthLen:0;}}
        public int HeightLen {get{return MapMgr?MapMgr.WidthLen:0;}}
        public void ClearMoveRange();
        // public void CheckColider();
        // public void getPathTransform(ref List<Vector3> path);
        // public void AddCursor(GameObject _Cursor);
        public void FillColor(List<Vector3> path,Color color);
    }
    public class MapSystem : AbstractSystem, IMapSystem
    {
        private IGameModel mModel;
        public MapManager MapMgr {get; set;}
        protected override void OnInit()
        {
            mModel = this.GetModel<IGameModel>();
        }

        
        // public void CheckColider()
        // {
        //     for(int i = 0;i<HeightLen;i++)
        //     {
        //         for(int j = 0; j < WidthLen; j++)
        //         {
        //             Vector3Int Cell = Tile2Cell(new Vector3Int(j, i, 0));
        //             int index = i * WidthLen + j;
        //             Vector3 Celltrans = Tilemaps.CellToWorld(Cell);
        //             Celltrans.x += 0.24f;
        //             Celltrans.y += 0.24f;
        //             Collider2D collider = Physics2D.OverlapBox(Celltrans, new Vector2(0.1f, 0.1f), 0, 1 << 3);
        //             if(TileInfo[index].GridType == (int)TileType.WALL)
        //             {
        //                 continue;
        //             }
        //             if (collider != null)
        //             {
        //                 TileInfo[index].ColiderBox = true;
        //             }
        //             else
        //             {
        //                 TileInfo[index].ColiderBox = false;
        //             }
        //         }
        //     }
        // }
        
        public void ClearMoveRange()
        {
            PathPool pathpool =  ResManager.Intance.PathPool; 
            foreach(var it in MapMgr.PathObjects)
            {
                pathpool.Release(it);          
            }
            MapMgr.PathObjects.Clear();        
        }
        // /// <summary>
        // /// 获取路径各个节点
        // /// </summary>
        // /// <param name="path"></param>
        // public void getPathTransform(ref List<Vector3> path)
        // {
        //     foreach (var it in PathList)
        //     {
        //         Vector3Int Tile = Vector3Int.zero;
        //         Tile.x = it % WidthLen;
        //         Tile.y = it / WidthLen;
                
        //         path.Add(Tile2World(Tile));
        //     }
        // }

        // public void AddCursor(GameObject _Cursor)
        // {
        //     if(Cursor == null)
        //     {
        //         Cursor = _Cursor;
        //     }
        //     else
        //     {
        //         Cursor.DestroySelf();
        //     }
        // }

        public void FillColor(List<Vector3> path,Color color)
        {
            foreach(Vector3 it in path)
            {
                GameObject poolmgr  =   ResManager.Intance.PathPoolMgr;
                PathPool pathpool =  ResManager.Intance.PathPool; 
                SpriteRenderer render =  pathpool.Get();
                render.gameObject.transform.SetParent(poolmgr.transform);
                render.color = color;
                render.transform.position = it;
                MapMgr.PathObjects.Add(render);      
            }  
        }
        
    }   


}
