using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class UIManager
    {
        Dictionary<string, GameObject> UI_GameObjectRes;
        Dictionary<string, GameObject> UI_GameObject;
        public UIManager()
        {
            UI_GameObjectRes = new() { };
            UI_GameObject = new() { };
        }
        public UIManager(Dictionary<string, GameObject> gameObjectRes, Dictionary<string, GameObject> ui_object)
        {
            UI_GameObjectRes = gameObjectRes;
            UI_GameObject = ui_object;
        }
        /// <summary>
        /// 获取或创建一个UI
        /// </summary>
        /// <param name="UI"></param>
        /// <returns></returns>
        public GameObject GetUI(string UI)
        {
            if (UI_GameObject.ContainsKey(UI))
            {
                //UI_GameObject[UI].SetActive(true);
                return UI_GameObject[UI];
            }

            GameObject ui = null;
            UI_GameObjectRes.TryGetValue(UI, out GameObject game);
            //Debug.Log(game);
            if (game != null)
            {
                ui = GameObject.Instantiate(game);
                ui.name = UI;
            }
            UI_GameObject.Add(UI, ui);
            return ui;
        }
        /// <summary>
        /// 摧毁UI
        /// </summary>
        /// <param name="UI"></param>
        public void DestoryUI(string UI)
        {
            UI_GameObject.TryGetValue(UI, out GameObject game);
            if (game != null)
            {
                game.DestroySelf();
            }
            UI_GameObject.Remove(UI);
        }
    }
}
