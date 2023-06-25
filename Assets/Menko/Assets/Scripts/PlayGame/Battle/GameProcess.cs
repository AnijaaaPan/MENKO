using UnityEngine;

namespace Menko.GameProcess
{
    [System.Serializable]
    public enum BattleUserType
    {
        Player,
        CPU,
        Field
    }

    [System.Serializable]
    public class BattleUserState
    {
        public BattleUserType UserType;
        public MenkoData SetMenkoData;
    }
}