using Menko.GameProcess;
using Menko.PlayerData;
using Menko.UpdateMenko;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ProcessWaitStart : MonoBehaviour
{

    [SerializeField]
    GameObject PlayerMenkoPreview;

    [SerializeField]
    GameObject CPUMenkoPreview;

    [SerializeField]
    GameObject InGameUIObject;

    private GameProcess GameProcess;

    private void Start()
    {
        GameProcess = GetComponent<GameProcess>();
    }

    public async void Run()
    {
        UpdateBattleUsers();
        UpdateUserMenkoPreview();

        await ShowInGameUI();
    }

    private async Task ShowInGameUI()
    {
        InGameUIObject.SetActive(true);
        CanvasGroup UICanvasGroup = InGameUIObject.GetComponent<CanvasGroup>();
        for (int i = 1; i <= 10; i++)
        {
            UICanvasGroup.alpha = i * 0.1f;
            await Task.Delay(50);
        }
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
        MenkoData getMenkoData = GameProcess.GetRandomMenkoObject();
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
