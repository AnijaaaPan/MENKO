using Menko.GameProcess;
using Menko.PlayerData;
using Menko.UpdateMenko;
using System.Collections.Generic;
using UnityEngine;

public class ProcessInit : MonoBehaviour
{
    [SerializeField]
    GameObject RingObject;

    [SerializeField]
    GameObject TitleMenkoObjects;

    [SerializeField]
    GameObject InGameMenkoObjects;

    [SerializeField]
    GameObject InitMenkoObject;

    [SerializeField]
    CameraRing CameraRing;

    public void Run()
    {
        CameraRing.WaitStart();

        TitleMenkoObjects.SetActive(false);
        InGameMenkoObjects.SetActive(true);

        InitBattleUsers();
        SetBattleMenko();
    }

    private void InitBattleUsers()
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
        MenkoData getMenkoData = GetRandomMenkoObject();
        return getMenkoData;
    }

    private MenkoData GetRandomMenkoObject(bool isAll = false)
    {
        PlayerData playerData = Json.instance.Load();
        List<MenkoData> menkoDatas = GameProcess.instance.MenkoDataBase.GetMenkos();
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

    private void SetBattleMenko()
    {
        MenkoData stageMenkoData = GetRandomMenkoObject(true);
        GameProcess.instance.StageMenko = stageMenkoData;

        Vector3 InitPos = InitMenkoObject.transform.localPosition;
        Quaternion InitQ = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);

        GameObject MenkoPrefab = stageMenkoData.GetPrefab();
        GameObject MenkoObject = Instantiate(MenkoPrefab, InitPos, InitQ);
        MenkoObject.name = "StageMenko";
        MenkoMesh.Update(MenkoObject, stageMenkoData);

        Outline MenkoOutline = MenkoObject.AddComponent<Outline>();
        MenkoOutline.OutlineColor = new(1, 1, 1, 0.75f);
        MenkoOutline.OutlineWidth = 2.5f;

        MenkoObject.SetActive(true);
        MenkoObject.transform.SetParent(InGameMenkoObjects.transform, false);
    }
}
