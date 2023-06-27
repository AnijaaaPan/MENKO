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
        WaitNextRound,
        EndGame
    }

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
        public ScriptableObjects.MenkoData SetMenkoData;
    }
}