using Menko.Enums;
using Menko.GameProcess;
using Menko.PlayerData;
using System.Collections.Generic;
using UnityEngine;

public class GameProcess : MonoBehaviour
{
    public static GameProcess instance;
    public CameraMultiTarget cameraMultiTarget;
    public MenkoDataBase MenkoDataBase;

    public ProcessState ProcessState = ProcessState.Init;
    public BattleUserType BattleTurn = BattleUserType.Player;
    public MenkoData StageMenko;
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

    public MenkoData GetRandomMenkoObject(bool isAll = false)
    {
        PlayerData playerData = Json.instance.Load();
        List<MenkoData> menkoDatas = MenkoDataBase.GetMenkos();
        List<MenkoData> filterPrefabs = menkoDatas.FindAll(data =>
        {
            if (isAll) return true;

            int id = data.GetId();
            MenkoAchievement menkoAchievement = playerData.MenkoAchievements.Find(m => m.id == id);
            return menkoAchievement.isOpen;
        });

        System.Random random = new();
        int randomIndex = random.Next(filterPrefabs.Count);
        return filterPrefabs[randomIndex];
    }

    public void InitSetCameraObject(GameObject[] Objects)
    {
        cameraMultiTarget.SetTargets(Objects);
    }

    public void UpdateProcessWaitStart()
    {
        ProcessState = ProcessState.WaitStart;
        ProcessWaitStart.Run();
    }
}
