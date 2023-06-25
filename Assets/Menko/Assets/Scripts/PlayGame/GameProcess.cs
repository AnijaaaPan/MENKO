using UnityEngine;
using Menko.Enums;
using Menko.GameProcess;
using System.Collections.Generic;

public class GameProcess : MonoBehaviour
{
    public static GameProcess instance;
    public MenkoDataBase MenkoDataBase;

    public ProcessState ProcessState = ProcessState.Init;
    public BattleUserType BattleTurn = BattleUserType.Player;
    public List<BattleUserState> BattleUsers;

    private ProcessInit ProcessInit;
    private ProcessWaitStart ProcessWaitStart;
    private ProcessFallPoint ProcessFallPoint;
    private ProcessFallPower ProcessFallPower;
    private ProcessMenkoFalling ProcessMenkoFalling;
    private ProcessMenkoFallEnd ProcessMenkoFallEnd;
    private ProcessWaitNextRound ProcessWaitNextRound;
    private ProcessEndGame ProcessEndGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ProcessInit = GetComponent<ProcessInit>();
        ProcessWaitStart = GetComponent<ProcessWaitStart>();
        ProcessFallPoint = GetComponent<ProcessFallPoint>();
        ProcessFallPower = GetComponent<ProcessFallPower>();
        ProcessMenkoFalling = GetComponent<ProcessMenkoFalling>();
        ProcessMenkoFallEnd = GetComponent<ProcessMenkoFallEnd>();
        ProcessWaitNextRound = GetComponent<ProcessWaitNextRound>();
        ProcessEndGame = GetComponent<ProcessEndGame>();

        ProcessInit.Run();
    }
}
