using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����״̬���ƽӿ�
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

    public virtual void StateBegin() { }//������Դ ��ʼ�� �жϸ���
    
    public virtual void StateEnd(){ }

    public virtual void StateUpdate() { }


}
