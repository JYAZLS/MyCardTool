using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace CardGameApp
{
    public class MapManager : MonoBehaviour,IController
    {
        public IGameModel GameData;
        public int WidthLen;
        public int HeightLen;
        private float cameraMoveSpeed = 1f;
        private float borderWidth = 10f;
        private float mapScaleMax = 2f;
        private float mapScaleMin = 1f;  
        public List<SpriteRenderer>  PathObjects;
        // Start is called before the first frame update
        void Start()
        {
            GameData = this.GetModel<IGameModel>();
            WidthLen = GameData.mapInfo.WidthLen;
            HeightLen = GameData.mapInfo.HeightLen;
            PathObjects = new();
        }

        // Update is called once per frame
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

        }
        public Vector3 ChangeWorldToTilePos(Vector3 mousePos)
        {
            return Cell2World(World2Cell(mousePos));
        }
        public Vector3 Tile2World(Vector3Int _vector)
        {
            Vector3 vector3 = Vector3.zero;
            vector3.x = (_vector.x - WidthLen/2) * 0.48f + 0.24f;
            vector3.y = (HeightLen/2 - _vector.y) * 0.48f - 0.24f;
            return vector3;
        }
        public static Vector3Int Cell2Tile(Vector3Int _vector,int WidthLen,int HeightLen)
        {
            Vector3Int vector3 = Vector3Int.zero;
            vector3.x = _vector.x  + WidthLen/2;
            vector3.y = -_vector.y - 1 + HeightLen/2;
            return vector3;
        }
        public static Vector3 Cell2World(Vector3Int _vector)
        {
            Vector3 vector3 = Vector3.zero;
            vector3.x =  _vector.x  * 0.48f + 0.24f;
            vector3.y =  _vector.y  * 0.48f + 0.24f;
            return vector3;
        }
        public static Vector3Int Tile2Cell(Vector3Int _vector,int WidthLen,int HeightLen)
        {
            Vector3Int vector3 = Vector3Int.zero;
            vector3.x = _vector.x - WidthLen/2;
            vector3.y = -_vector.y + HeightLen/2 - 1;
            return vector3;
        }
        public static Vector3Int World2Cell(Vector3 _vector)
        {
            Vector3Int vector3 = Vector3Int.zero;
            vector3.x = (_vector.x > 0)?(int)(_vector.x / 0.48f):((int)(_vector.x / 0.48f) - 1);
            vector3.y = (_vector.y > 0)?(int)(_vector.y / 0.48f):((int)(_vector.y / 0.48f) - 1);
            return vector3;
        }
        public static Vector3Int World2Tile(Vector3 _vector,int WidthLen,int HeightLen)
        {
            Vector3Int vector3 = Vector3Int.zero;
            vector3 = World2Cell(_vector);
            vector3 = Cell2Tile(vector3,WidthLen,HeightLen);
            return vector3;
        }
        public static int  World2Index(Vector3 _vector,int WidthLen,int HeightLen)
        {
            int id = -1;
            Vector3Int vector3 = Vector3Int.zero;
            vector3 = World2Cell(_vector);
            vector3 = Cell2Tile(vector3,WidthLen,HeightLen);
            id = vector3.y * WidthLen + vector3.x;
            // Debug.Log(id);
            return id;
        }
        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }
}