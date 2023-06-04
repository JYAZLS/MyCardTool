using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class PlayerController : MonoBehaviour,IController
    {
        public UnitBase SelectPlayer;
        IMapSystem MapManager;     
        ABPath ABPathMgr = new();
        [SerializeField]
        private bool isReady = false;

        private void Start() {
            MapManager = this.GetSystem<IMapSystem>();
        }
        public void Updated()
        {
            if(Input.GetMouseButtonDown(1))
            {
                MapManager.ClearMoveRange();
                SelectPlayer = null;
                isReady = false;
                
            } 
            if(SelectPlayer != null && !isReady)
            {               
                List<Vector3>CanMovePathNode = new();   
                ABPathMgr.ShowMoveRange(SelectPlayer,ref CanMovePathNode,MapManager.WidthLen,MapManager.HeightLen);
                MapManager.FillColor(CanMovePathNode,Color.green);
                isReady = true;
            }
            if(isReady)
            {
                Vector3 PlayerPos = SelectPlayer.gameObject.transform.position;
                List<Vector3>CanMovePathNode = new();
                CanMovePathNode = ABPathMgr.GetMoveRange();
                //Debug.Log(CanMovePathNode.Count);
                MapManager.FillColor(CanMovePathNode,Color.green);
                CanMovePathNode.Clear();
                Vector3 worldpos = InputHandle.Intance.InputVector3;
                // Vector3 MousePos = Input.mousePosition;
                // UnityEngine.Vector3 worldpos = Camera.main.ScreenToWorldPoint(MousePos);
                ABPathMgr.ShowMovePath(PlayerPos,worldpos,ref CanMovePathNode);
                MapManager.FillColor(CanMovePathNode,Color.blue);             
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }

}