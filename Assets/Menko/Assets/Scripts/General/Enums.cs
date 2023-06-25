namespace Menko.Enums
{
    public enum Rank
    {
        Default,
        Common,
        Rare,
        Secret
    }

    public enum ProcessState
    {
        Init,
        WaitStart,
        FallPoint,
        FallPower,
        MenkoFalling,
        MenkoFallEnd,
        WaitNextRound,
        EndGame
    }

}