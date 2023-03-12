using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ActionScript : MonoBehaviour
{
    [HideInInspector] float MoveSpeed = 2f;
    public UnityAction MoveEnd;
    public IEnumerator MoveTo(Transform _trans,List<Vector3> path)
    {
        //Debug.Log("Start Move To");
        foreach (var it in path)
        {
            //Debug.Log(it);
            while (_trans.transform.position != it)
            {
                _trans.position = Vector2.MoveTowards(_trans.position, it, MoveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0);
            }
        }
        MoveEnd.Invoke();
    }
}
