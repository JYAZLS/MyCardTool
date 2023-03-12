using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CardGameApp
{
    public class LoadCharacterInfoEndCommand : AbstractCommand
    {
        private string[] CharacterInfoStream;
        public struct LoadCharacterInfoEndEvent { };
        public LoadCharacterInfoEndCommand(string[] data)
        {
            CharacterInfoStream = data;
        }
        protected override void OnExecute()
        {
            IGameSystem system = this.GetSystem<IGameSystem>();
            system.LoadCharacterInfo = CharacterInfoStream;
            this.SendEvent<LoadCharacterInfoEndEvent>();
        }
    }
}
