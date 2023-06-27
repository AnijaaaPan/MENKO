namespace Menko.GameProcess
{
    [System.Serializable]
    public enum ProcessState
    {
        Init,
        WaitStart,
        FallPointAndPower,
        MenkoFalling,
        MenkoFallEnd,
        NextRound,
        EndGame
    }

    [System.Serializable]
    public enum BattleUserType
    {
        Player,
        CPU,
        Stage
    }

    [System.Serializable]
    public class BattleUserState
    {
        public BattleUserType UserType;
        public ScriptableObjects.MenkoData SetMenkoData;
    }
}