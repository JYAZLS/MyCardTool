using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameApp
{
    public enum ProcessStatus {None , SetCharacter, Move, Moving, MoveEnd, Command, End}
    public static class ProcessManager
    {
        // Start is called before the first frame update     
        public static ProcessStatus Status = ProcessStatus.None;
        public static ICharacter Hero = null;
        public static bool SettingMode = false;

    }
}

