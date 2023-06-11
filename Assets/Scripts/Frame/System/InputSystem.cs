using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace CardGameApp
{
    public interface IInputSystem : ISystem
    {
        public InputHandle handle {get; set;}  
    }
    public class InputSystem : AbstractSystem, IInputSystem
    {
        public InputHandle handle {get; set;}      
        protected override void OnInit()
        {
        }

    }
}