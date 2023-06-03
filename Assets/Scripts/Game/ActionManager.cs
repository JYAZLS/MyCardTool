using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;
namespace CardGameApp
{
    public class ActionManager : UnitySingleton<ActionManager>
    {
        [SerializeField] 
        float MoveSpeed = 2f;
        [SerializeField] 
        public Vector3 LastPosition;
        
        public void MoveToAction(Transform _trans,List<Vector3> path)
        {
            Debug.Log("stop Coroutines");
            this.StopAllCoroutines();
            LastPosition = _trans.position;
            //MoveTo(_trans,path).ToAction().Start(this);
            //this.StartCoroutine(MoveTo(_trans,path));
            MoveTo(_trans,path);
        }

        public void MoveTo(Transform _trans,List<Vector3> path)
        {
            ActionKit.Custom( custom =>{
                custom.OnStart(()=> {_trans.DOPath(path.ToArray(),path.Count,PathType.Linear,PathMode.TopDown2D,3,null)
                .OnComplete(custom.Finish);});
                custom.OnFinish(()=>{
                    Debug.Log("On Finish");
                    // ProcessManager.Status = ProcessStatus.MoveEnd;
                });
            })
            .Start(this);
        }
    }
}
