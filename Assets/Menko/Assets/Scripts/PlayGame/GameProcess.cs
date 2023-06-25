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
    private ProcessFallPointAndPower ProcessFallPointAndPower;
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
        ProcessFallPointAndPower = GetComponent<ProcessFallPointAndPower>();
        ProcessMenkoFalling = GetComponent<ProcessMenkoFalling>();
        ProcessMenkoFallEnd = GetComponent<ProcessMenkoFallEnd>();
        ProcessWaitNextRound = GetComponent<ProcessWaitNextRound>();
        ProcessEndGame = GetComponent<ProcessEndGame>();

        UpdateProcessInit();
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

    public void UpdateProcessInit()
    {
        ProcessState = ProcessState.Init;
        ProcessInit.enabled = true;
        ProcessInit.Run();
    }

    public void UpdateProcessWaitStart()
    {
        ProcessInit.enabled = false;

        ProcessState = ProcessState.WaitStart;
        ProcessWaitStart.enabled = true;
        ProcessWaitStart.Run();
    }

    public void UpdateProcessFallPointAndPower()
    {
        ProcessWaitStart.enabled = false;

        ProcessState = ProcessState.FallPointAndPower;
        ProcessFallPointAndPower.enabled = true;
        ProcessFallPointAndPower.Run();
    }

    public void UpdateProcessMenkoFalling()
    {
        ProcessFallPointAndPower.enabled = false;

        ProcessState = ProcessState.MenkoFalling;
        ProcessMenkoFalling.enabled = true;
        ProcessMenkoFalling.Run();
    }

}
