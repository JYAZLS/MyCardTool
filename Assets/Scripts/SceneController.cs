using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 场景管理
/// </summary>
public class SceneController:UnitySingleton<SceneController>
{
    // Start is called before the first frame update
    private ISceneState m_sceneState;
    //private AsyncOperation m_asyncOperation;
    private bool isBegin;
    public void SetState(ISceneState state, string LoadSceneName)
    {
        LoadScene(LoadSceneName);
        m_sceneState?.StateEnd();
        m_sceneState = state;
    }
    private void LoadScene(string LoadSceneName)
    {
        if (LoadSceneName == null || LoadSceneName.Length == 0)
        {
            return;
        }
        isBegin = true;
        //m_asyncOperation = SceneManager.LoadSceneAsync(LoadSceneName);//异步加载
    }
    public void StateUpdate()
    {
        /*if ((m_asyncOperation != null) && (m_asyncOperation.isDone == false))
        {
            return;
        }
        if (m_asyncOperation != null && m_asyncOperation.isDone)
        {
            m_sceneState.StateBegin();
            m_asyncOperation = null;
        }*/
        if (isBegin)
        {
            isBegin = false;
            m_sceneState?.StateBegin();
        }
        else
        {
            m_sceneState?.StateUpdate();
        }
    }
}

