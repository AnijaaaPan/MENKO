using Menko.GameProcess;
using Menko.MenkoData;
using Menko.PlayerData;
using Menko.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

public class ProcessWaitStart : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerMenkoPreview;

    [SerializeField]
    GameObject CPUMenkoPreview;

    [SerializeField]
    GameObject InGameUIObject;

    [SerializeField]
    GameObject JoyStickObject;

    private void Update()
    {
        if (GameProcess.instance.ProcessState != ProcessState.WaitStart) return;
        if (GameProcess.instance.BattleTurn != BattleUserType.Player) return;

        bool isJoyStickActive = JoyStickObject.activeInHierarchy;
        if (!isJoyStickActive) return;

        CameraRing.instance.Stop();
        GameProcess.instance.UpdateProcessFallPointAndPower();
    }

    public void Run()
    {
        UpdateBattleUsers();
        UpdateUserMenkoPreview();

        InGameUIObject.SetActive(true);
        CameraRing.instance.WaitStart();
    }

    private void UpdateBattleUsers()
    {
        List<BattleUserState> BattleUsers = new();
        BattleUsers.Add(InitBattlePlayer());
        BattleUsers.Add(InitBattleCPU());

        GameProcess.instance.BattleUsers = BattleUsers;
    }

    private BattleUserState InitBattlePlayer()
    {
        BattleUserState BattleUser = new()
        {
            UserType = BattleUserType.Player,
            SetMenkoData = GetPlayerMenkoData(),
        };
        return BattleUser;
    }

    private MenkoData GetPlayerMenkoData()
    {
        PlayerData playerData = Json.instance.Load();
        MenkoData getMenkoData = GameProcess.instance.MenkoDataBase.GetMenko(playerData.SetMenkoId);
        return getMenkoData;
    }

    private BattleUserState InitBattleCPU()
    {
        BattleUserState BattleUser = new()
        {
            UserType = BattleUserType.CPU,
            SetMenkoData = GetCPUMenkoData()
        };
        return BattleUser;
    }

    private MenkoData GetCPUMenkoData()
    {
        MenkoData getMenkoData = GameProcess.instance.GetRandomMenkoObject();
        return getMenkoData;
    }

    private void UpdateUserMenkoPreview()
    {
        GameProcess.instance.BattleUsers.ForEach(user =>
        {
            bool isPlayer = user.UserType == BattleUserType.Player;
            GameObject previewObject = isPlayer ? PlayerMenkoPreview : CPUMenkoPreview;
            MenkoMesh.Update(previewObject, user.SetMenkoData);
        });
    }
}
