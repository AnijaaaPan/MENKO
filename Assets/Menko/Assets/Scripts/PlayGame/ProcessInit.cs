using UnityEngine;
using Menko.Enums;
using Menko.PlayerData;
using Menko.GameProcess;
using System.Collections.Generic;

[System.Serializable]
public class PosMenko
{
    public float MinX;
    public float MaxX;
    public float MinZ;
    public float MaxZ;
}

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

    private List<PosMenko> PosMenkos = new();

    public void Run()
    {
        CameraRing.WaitStart();

        TitleMenkoObjects.SetActive(false);
        InGameMenkoObjects.SetActive(true);

        List<MenkoData> menkoPrefabs = GameProcess.instance.MenkoDataBase.GetMenkos();
        InitBattleUsers(menkoPrefabs);
        SetBattleMenkos();
    }

    private void InitBattleUsers(List<MenkoData> menkoPrefabs)
    {
        List<BattleUserState> BattleUsers = new();
        BattleUsers.Add(InitBattlePlayer());
        BattleUsers.Add(InitBattleCPU(menkoPrefabs));

        GameProcess.instance.BattleUsers = BattleUsers;
    }

    private BattleUserState InitBattlePlayer()
    {
        BattleUserState BattleUser = new()
        {
            UserType = BattleUserType.Player,
            MainMenkoData = GetPlayerMenkoData(Setting.Main),
            SubMenkoData = GetPlayerMenkoData(Setting.Sub)
        };
        return BattleUser;
    }

    private MenkoData GetPlayerMenkoData(Setting SettingType)
    {
        PlayerData playerData = Json.instance.Load();
        int getMenkoId = playerData.GetMenkoSettingByIndex(SettingType).id;

        MenkoData getMenkoData = GameProcess.instance.MenkoDataBase.GetMenko(getMenkoId);
        return getMenkoData;
    }

    private BattleUserState InitBattleCPU(List<MenkoData> menkoPrefabs)
    {
        BattleUserState BattleUser = new()
        {
            UserType = BattleUserType.CPU,
            MainMenkoData = GetCPUMenkoData(menkoPrefabs),
            SubMenkoData = GetCPUMenkoData(menkoPrefabs)
        };
        return BattleUser;
    }

    private MenkoData GetCPUMenkoData(List<MenkoData> menkoPrefabs)
    {
        MenkoData getMenkoData = GetRandomMenkoObject(menkoPrefabs);
        return getMenkoData;
    }

    private MenkoData GetRandomMenkoObject(List<MenkoData> menkoPrefabs)
    {
        System.Random random = new();
        int randomIndex = random.Next(menkoPrefabs.Count);
        return menkoPrefabs[randomIndex];
    }

    private void SetBattleMenkos()
    {
        List<BattleUserState> battleUsers = GameProcess.instance.BattleUsers;
        battleUsers.ForEach(battleUser =>
        {
            SetBattleMenko(battleUser.MainMenkoData, battleUser.UserType);
        });
    }

    private void SetBattleMenko(MenkoData MainMenkoData, BattleUserType type)
    {
        Vector3 InitPos = InitRandomPos();
        Quaternion InitQ = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);

        string typeName = System.Enum.GetName(typeof(BattleUserType), type);

        GameObject MenkoPrefab = MainMenkoData.GetPrefab();
        GameObject MenkoObject = Instantiate(MenkoPrefab, InitPos, InitQ);
        MenkoObject.name = $"{typeName}_MainMenko";
        UpdateMenkoObject(MenkoObject, MenkoPrefab, MainMenkoData, type);

        Color outlineColor = type == BattleUserType.Player ? Color.blue : Color.red;
        Outline MenkoOutline = MenkoObject.AddComponent<Outline>();
        MenkoOutline.OutlineColor = outlineColor;

        MenkoObject.SetActive(true);
        MenkoObject.transform.SetParent(InGameMenkoObjects.transform, true);
    }

    private Vector3 InitRandomPos()
    {
        float MenkoSize = 6;

        while (true)
        {
            float RandomRadius = Random.Range(3f, 7f);
            Vector3 SpawnPos = RandomRadius * Random.insideUnitSphere + RingObject.transform.localPosition;

            float RandomX = SpawnPos.x;
            float RandomZ = SpawnPos.z;
            if (CheckDistinctDice(RandomX, RandomZ)) continue;

            PosMenko PosMenko = new()
            {
                MinX = RandomX - MenkoSize,
                MaxX = RandomX + MenkoSize,
                MinZ = RandomZ - MenkoSize,
                MaxZ = RandomZ + MenkoSize
            };
            PosMenkos.Add(PosMenko);
            return new Vector3(RandomX, 2.25f, RandomZ);
        }
    }

    private bool CheckDistinctDice(float RandomX, float RandomZ)
    {
        for (int i = 0; i < PosMenkos.Count; i++)
        {
            PosMenko PosMenko = PosMenkos[i];
            if (PosMenko.MinX <= RandomX && RandomX <= PosMenko.MaxX &&
                PosMenko.MinZ <= RandomZ && RandomZ <= PosMenko.MaxZ)
                return true;
        }
        return false;
    }

    private void UpdateMenkoObject(GameObject targetObject, GameObject targetPrefab, MenkoData data, BattleUserType type)
    {
        Rank MenkoRank = data.GetRank();
        UpdateLocalScale(targetObject, data);

        GameObject upObject = FindObject(targetObject, "Up");
        GameObject downObject = FindObject(targetObject, "Down");
        GameObject sideObject = FindObject(targetObject, "Side");

        GameObject upPrefab = FindObject(targetPrefab, "Up");
        GameObject downPrefab = FindObject(targetPrefab, "Down");
        GameObject sidePrefab = FindObject(targetPrefab, "Side");

        GameObject updateUpObject = MenkoRank == Rank.Default ? downPrefab : upPrefab;
        GameObject updateDownPrefab = MenkoRank == Rank.Default ? upPrefab : downPrefab;

        UpdateMesh(upObject, updateUpObject);
        UpdateMaterials(upObject, updateUpObject, data);

        UpdateMesh(downObject, updateDownPrefab);
        UpdateMaterials(downObject, updateDownPrefab, data);

        UpdateMesh(sideObject, sidePrefab);
        UpdateMaterials(sideObject, sidePrefab, data);
    }

    private void UpdateMesh(GameObject targetObject, GameObject targetPrefab)
    {
        Mesh sharedMesh = targetPrefab.GetComponent<MeshFilter>().sharedMesh;
        MeshFilter targetMeshFilter = targetObject.GetComponent<MeshFilter>();
        targetMeshFilter.mesh = sharedMesh;

        if (targetObject.name != "Side")
        {
            targetObject.GetComponent<MeshCollider>().enabled = false;
            return;
        }
        MeshCollider targetMeshCollider = targetObject.GetComponent<MeshCollider>();
        targetMeshCollider.sharedMesh = sharedMesh;
        targetMeshCollider.convex = true;
    }

    private void UpdateMaterials(GameObject targetObject, GameObject targetPrefab, MenkoData data)
    {
        Material[] sharedMaterials = targetPrefab.GetComponent<MeshRenderer>().sharedMaterials;
        MeshRenderer targetMeshRenderer = targetObject.GetComponent<MeshRenderer>();

        if (!data.GetIsAutoMosaic())
        {
            targetMeshRenderer.materials = sharedMaterials;
            return;
        }

        PlayerData playerData = Json.instance.Load();
        if (!playerData.CheckAllAchievementOpen())
        {
            targetMeshRenderer.materials = sharedMaterials;
            return;
        };

        targetMeshRenderer.material = null;
        targetMeshRenderer.material = sharedMaterials[0];
    }

    private void UpdateLocalScale(GameObject targetObject, MenkoData data)
    {
        int MenkoId = data.GetId();

        if (MenkoId == 1)
        {
            targetObject.transform.localScale *= 0.775f;
        }

        if (MenkoId > 3)
        {
            float newX = targetObject.transform.localScale.x;
            float newY = targetObject.transform.localScale.y * 0.725f;
            float newZ = targetObject.transform.localScale.z;
            targetObject.transform.localScale = new Vector3(newX, newY, newZ);
        }
    }

    private GameObject FindObject(GameObject parent, string name)
    {
        return parent.transform.Find(name).gameObject;
    }
}
