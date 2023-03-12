using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public class LoadMapInfoEndCommand : AbstractCommand
    {
        private string[] MapInfoStream;
        public struct LoadMapInfoEndEvent { };

        public LoadMapInfoEndCommand(string[] data)
        {
            MapInfoStream = data;
        }

        protected override void OnExecute()
        {
            IGameSystem system = this.GetSystem<IGameSystem>();
            system.LoadMapInfo = MapInfoStream;
            this.SendEvent<LoadMapInfoEndEvent>();
        }
    }
}
