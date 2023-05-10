using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class CharacterFactory : MonoBehaviour
    {
        public GameObject SetPlayer;
        void Start()
        {
            
        }
        void Update()
        {
            Vector3 MousePos = Input.mousePosition;
            UnityEngine.Vector3 worldpos = Camera.main.ScreenToWorldPoint(MousePos);
            if(SetPlayer != null)
            {
                DragCharacter(SetPlayer,worldpos);
                if(Input.GetMouseButtonDown(1))
                {
                    SetPlayer = null;
                }
            }
            
        }
        public void DragCharacter(GameObject hero,Vector3 vector3)
        {
            hero.transform.position = new Vector3(vector3.x + 0.24f, vector3.y + 0.24f, 0);
        }
    }
}