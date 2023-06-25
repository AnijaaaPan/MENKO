using UnityEngine;

namespace Menko.GameProcess
{
    [System.Serializable]
    public enum BattleUserType
    {
        Player,
        CPU
    }

    [System.Serializable]
    public class BattleUserState
    {
        public BattleUserType UserType;
        public MenkoData SetMenkoData;
    }
}