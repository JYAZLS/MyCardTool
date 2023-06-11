using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventSystem<T0,T1>: MonoBehaviour
{
    public static Dictionary<T0 ,UnityEvent<T1>> EventDictionary = new Dictionary<T0, UnityEvent<T1>>();

    /// <summary>
    /// 监听一个参数
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="Listener"></param>
    public void StartListening(T0 EventName, UnityAction<T1> Listener)
    {
        UnityEvent<T1> unityEvent = null;
        if (EventDictionary.TryGetValue(EventName, out unityEvent))
        {
            unityEvent.AddListener(Listener);
        }
        else
        {
            unityEvent = new UnityEvent<T1>();
            unityEvent.AddListener(Listener);
            EventDictionary.Add(EventName, unityEvent);
        }
    }

    /// <summary>
    /// 停止监听
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="Listener"></param>
    public void StopListening(T0 EventName, UnityAction<T1> Listener)
    {
        UnityEvent<T1> unityEvent = null;
        if (EventDictionary.TryGetValue(EventName, out unityEvent))
        {
            unityEvent.RemoveListener(Listener);
            if (unityEvent.GetPersistentEventCount() == 0)
            {
                EventDictionary.Remove(EventName);
            }
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="sender"></param>
    public void TriggerEvent(T0 EventName, T1 sender)
    {
        UnityEvent<T1> unityEvent = null;
        if (EventDictionary.TryGetValue(EventName, out unityEvent))
        {
            Debug.Log(EventName + " TriggerEvent");
            unityEvent.Invoke(sender);
        }
        else
        {
            Debug.Log(EventName + " not this Event");
        }
    }

}
public class GameEventSystem<T0, T1, T2> : MonoBehaviour
{
    public static Dictionary<T0, UnityEvent<T1, T2>> EventDictionary = new Dictionary<T0, UnityEvent<T1, T2>>();

    /// <summary>
    /// 监听一个参数
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="Listener"></param>
    public void StartListening(T0 EventName, UnityAction<T1, T2> Listener)
    {
        UnityEvent<T1, T2> unityEvent = null;
        if (EventDictionary.TryGetValue(EventName, out unityEvent))
        {
            unityEvent.AddListener(Listener);
        }
        else
        {
            unityEvent = new UnityEvent<T1, T2>();
            unityEvent.AddListener(Listener);
            EventDictionary.Add(EventName, unityEvent);
        }
    }

    /// <summary>
    /// 停止监听
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="Listener"></param>
    public void StopListening(T0 EventName, UnityAction<T1, T2> Listener)
    {
        UnityEvent<T1, T2> unityEvent = null;
        if (EventDictionary.TryGetValue(EventName, out unityEvent))
        {
            unityEvent.RemoveListener(Listener);
            if (unityEvent.GetPersistentEventCount() == 0)
            {
                EventDictionary.Remove(EventName);
            }
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="sender"></param>
    public void TriggerEvent(T0 EventName, T1 sender1, T2 sender2)
    {
        UnityEvent<T1, T2> unityEvent = null;
        if (EventDictionary.TryGetValue(EventName, out unityEvent))
        {
            //Debug.Log(EventName + " TriggerEvent");
            unityEvent.Invoke(sender1,sender2);
        }
        else
        {
            //Debug.Log(EventName + " not this Event");
        }
    }

}
public class GameEventSystem<T0, T1, T2, T3> : MonoBehaviour
{
    public static Dictionary<T0, UnityEvent<T1, T2, T3>> EventDictionary = new Dictionary<T0, UnityEvent<T1, T2, T3>>();

    /// <summary>
    /// 监听一个参数
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="Listener"></param>
    public void StartListening(T0 EventName, UnityAction<T1, T2, T3> Listener)
    {
        UnityEvent<T1, T2, T3> unityEvent = null;
        if (EventDictionary.TryGetValue(EventName, out unityEvent))
        {
            unityEvent.AddListener(Listener);
        }
        else
        {
            unityEvent = new UnityEvent<T1, T2, T3>();
            unityEvent.AddListener(Listener);
            EventDictionary.Add(EventName, unityEvent);
        }
    }

    /// <summary>
    /// 停止监听
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="Listener"></param>
    public void StopListening(T0 EventName, UnityAction<T1, T2, T3> Listener)
    {
        UnityEvent<T1, T2, T3> unityEvent = null;
        if (EventDictionary.TryGetValue(EventName, out unityEvent))
        {
            unityEvent.RemoveListener(Listener);
            if (unityEvent.GetPersistentEventCount() == 0)
            {
                EventDictionary.Remove(EventName);
            }
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="sender"></param>
    public void TriggerEvent(T0 EventName, T1 sender1, T2 sender2, T3 sender3)
    {
        UnityEvent<T1, T2, T3> unityEvent = null;
        if (EventDictionary.TryGetValue(EventName, out unityEvent))
        {
            Debug.Log(EventName + " TriggerEvent");
            unityEvent.Invoke(sender1, sender2, sender3);
        }
        else
        {
            Debug.Log(EventName + " not this Event");
        }
    }

}