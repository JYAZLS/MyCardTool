using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CardGameApp
{
    public class ABPath
    {
        ICharacter character;
        int Width = 0;
        int Height = 0;
        List<int> OpenList = new();
        List<int> CloseList = new();
        private List<int> Direct = new(){-1, 0,1 , 0};
        Func<int> m_CostMove;
        Func<int,bool> m_CanMove;
        public void ShowMoveRange(ICharacter _character,ref List<Vector3>GetCanMovePathNode,int _Width,int _Height)
        {
            CloseList.Clear();
            OpenList.Clear();
            character = _character;
            Width = _Width;
            Height = _Height;
            Direct[1] = -Width;
            Direct[3] = Width;
            int start = Vec3ToID(_character.mGameObject.transform.position);
            findNearNode(start,_character.Military.MoveRange);
            //Debug.Log(_character.Military.MoveRange);
            foreach(int it in OpenList)
            {
                GetCanMovePathNode.Add(IDToVector3(it));
            }
        }

        void findNearNode(int p_start,int MoveRange)
        {
            int id = p_start;
            if(MoveRange <= 0)
            {
                return;
            }       
            foreach(int it in Direct)
            {
                id = p_start+it;
                Debug.Log("p_start:"+p_start);
                Debug.Log("it:"+it);
                Debug.Log("id:"+id);
                if(CalculateFn(p_start,id) <= 1)//要么同一行要么同一列
                {
                    if(NodeJudge(id))
                    {  
                        int cost = 1;
                        if(m_CostMove!=null)
                        {
                            cost = m_CostMove.Invoke();
                        }
                        findNearNode(id,MoveRange - cost);         
                    }
                }
            }
        }    

        
        public void SetNodeIsCanMove(Func<int,bool> func)
        {
            m_CanMove = func;
        }

        public void CalculateMoveCost(Func<int> CalMoveCost)
        {
            //花费函数
            m_CostMove = CalMoveCost;
        }

        private bool NodeJudge(int id) 
        {
            if(!CloseList.Contains(id))
            {  
                bool isCanMove = true;
                if(m_CanMove != null)
                {
                    isCanMove = m_CanMove.Invoke(id);
                }
                if(isCanMove)
                {
                    if(!OpenList.Contains(id))               
                        OpenList.Add(id);
                    return true;
                }  
                else
                {
                    CloseList.Add(id);
                    return false;
                }                
            }
            return false;
        }
        int Vec3ToID(Vector3 pos)
        {
            int id = -1;
            Vector3Int vector3 = Vector3Int.zero;
            vector3.x = (pos.x > 0)?(int)(pos.x / 0.48f):((int)(pos.x / 0.48f) - 1);
            vector3.y = (pos.y > 0)?(int)(pos.y / 0.48f):((int)(pos.y / 0.48f) - 1);
            vector3.x = vector3.x  + Width/2;
            vector3.y = -vector3.y - 1 + Height/2;
            id = vector3.y * Width + vector3.x;
            return id;
        }

        Vector3 IDToVector3(int id)
        {
            Vector3 vector3 = Vector3.zero;
            vector3.x = id % Width;
            vector3.y = (int)(id / Width);

            vector3.x = (vector3.x - Width/2) * 0.48f + 0.24f;
            vector3.y = (Height/2 - vector3.y) * 0.48f - 0.24f;

            return vector3;
        }

        public void ShowMovePath(Vector3 FromVec3,Vector3 ToVec3,ref List<Vector3> GetCanMovePathNode)
        {
            CloseList.Clear();
            int p_start = Vec3ToID(FromVec3);
            int p_end = Vec3ToID(ToVec3);
            bool CalFlag = true;
            List<int> PathNode = new();
            Dictionary<int, int> HnValuePairs = new Dictionary<int, int>();
            if(!OpenList.Contains(p_end))
            {
                return;
            }
            if(p_start == p_end)//选择自身
            {
                GetCanMovePathNode.Add(IDToVector3(p_start));
                return;
            }
            PathNode.Add(p_start);
            CloseList.Add(p_start);
            while(CalFlag)
            {
                foreach(int it in Direct)//计算Fn
                {
                    int id = p_start + it;   
                    if(OpenList.Contains(id) && !CloseList.Contains(id))
                    {
                        int Fn = CalculateFn(id,p_end);
                        HnValuePairs.Add(id,Fn);
                        CloseList.Add(id);
                    }
                }
                //找出最小的Hn
                int Minid = -1;
                int MinHn = -1;
                foreach(var it in HnValuePairs)
                {
                    if (Minid == -1)
                    {
                        Minid = it.Key;
                        MinHn = it.Value;
                    }
                    else
                    {
                        if (it.Value <= MinHn)
                        {
                            Minid = it.Key;
                            MinHn = it.Value;
                        }
                    }
                }
                //Debug.Log(Minid);
                if(PathNode.Count == 1)
                {
                    PathNode.Add(Minid);
                }
                else{
                    for(int i = PathNode.Count-1;i>0;i--)//找到连续的节点
                    {
                        int  Diff = CalculateFn(PathNode[i],Minid);
                        if(Diff > 1)
                        {
                            PathNode.RemoveAt(i);//非连续节点
                        }
                        else{
                            PathNode.Add(Minid);
                            break;
                        }
                    }
                }
                p_start = Minid;//切换下个节点,新一轮计算
                //Debug.Log("minid:"+Minid);
                if(p_start == p_end)
                {
                    CalFlag = false; //结束计算
                    //Debug.Log("p_start:"+p_start+" p_end:"+p_end+" size:"+PathNode.Count);
                    foreach(int id in PathNode)
                    {
                        GetCanMovePathNode.Add(IDToVector3(id));
                    }
                }
            }
            
        }

        int CalculateFn(int pos1,int pos2)
        {
            return Mathf.Abs(pos1/Width - pos2/Width) + Mathf.Abs(pos1%Width - pos2%Width); 
        }

        public List<Vector3> GetMoveRange()
        {
            List<Vector3> GetCanMovePathNode = new();
            foreach(int it in OpenList)
            {
                GetCanMovePathNode.Add(IDToVector3(it));
            }
            return GetCanMovePathNode;
        }
    }
}