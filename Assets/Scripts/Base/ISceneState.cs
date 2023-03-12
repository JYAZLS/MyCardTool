using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景状态控制接口
/// </summary>
public class ISceneState
{
    private string m_GameStateName = "IScenceState";

    public string StateName
    {
        get { return m_GameStateName; }
        set { m_GameStateName = value; }
    }
    protected SceneController m_Controller = null;

    public ISceneState() { }

    public ISceneState(SceneController SceneState)
    {
        m_Controller = SceneState;
    }

    public virtual void StateBegin() { }//加载资源 初始化 判断更新
    
    public virtual void StateEnd(){ }

    public virtual void StateUpdate() { }


}
