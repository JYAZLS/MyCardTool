using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class PlayerController : MonoBehaviour,IController
    {
        public UnitBase SelectPlayer;
        IMapSystem MapSys;
        IInputSystem InputSys;     
        ABPath ABPathMgr = new();
        [SerializeField]
        private bool isReady = false;

        private void Start() {
            MapSys = this.GetSystem<IMapSystem>();
            InputSys = this.GetSystem<IInputSystem>();
        }
        public void Updated()
        {
            if(Input.GetMouseButtonDown(1))
            {
                MapSys.ClearMoveRange();
                SelectPlayer = null;
                isReady = false;
                
            } 
            if(SelectPlayer != null && !isReady)
            {               
                List<Vector3>CanMovePathNode = new();   
                ABPathMgr.ShowMoveRange(SelectPlayer,ref CanMovePathNode,MapSys.WidthLen,MapSys.HeightLen);
                MapSys.FillColor(CanMovePathNode,Color.green);
                isReady = true;
            }
            if(isReady)
            {
                Vector3 PlayerPos = SelectPlayer.gameObject.transform.position;
                List<Vector3>CanMovePathNode = new();
                CanMovePathNode = ABPathMgr.GetMoveRange();
                //Debug.Log(CanMovePathNode.Count);
                MapSys.FillColor(CanMovePathNode,Color.green);
                CanMovePathNode.Clear();
                Vector3 worldpos = InputSys.handle.InputVector3;
                // Vector3 MousePos = Input.mousePosition;
                // UnityEngine.Vector3 worldpos = Camera.main.ScreenToWorldPoint(MousePos);
                ABPathMgr.ShowMovePath(PlayerPos,worldpos,ref CanMovePathNode);
                MapSys.FillColor(CanMovePathNode,Color.blue);             
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameFrame.Interface;
        }
    }

}