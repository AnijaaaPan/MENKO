using Menko.Enums;
using Menko.PlayerData;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSlideMenu : MonoBehaviour
{
    [SerializeField]
    GameObject InitMenkoObject;

    [SerializeField]
    MenkoDataBase MenkoDataBase;

    //[SerializeField]
    //GameObject ;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        PlayerData playerData = Json.instance.Load();
        List<MenkoData> menkoDatas = MenkoDataBase.GetMenkos();
        menkoDatas.ForEach(data =>
        {
            GenerateMenkoObject(playerData, data);
        });
    }

    private void GenerateMenkoObject(PlayerData playerData, MenkoData data)
    {
        int menkoId = data.GetId();
        GameObject newMenkoObject = Instantiate(InitMenkoObject);
        newMenkoObject.name = $"Menko {menkoId}";
        newMenkoObject.transform.SetParent(transform, false);

        GenerateMenkoPreviewObject(playerData, newMenkoObject, data);
        newMenkoObject.SetActive(true);
    }

    private void GenerateMenkoPreviewObject(PlayerData playerData, GameObject newMenkoObject, MenkoData data)
    {
        int menkoId = data.GetId();
        Rank MenkoRank = data.GetRank();
        GameObject MenkoPrefab = data.GetPrefab();

        MenkoAchievement menkoAchievement = playerData.GetMenkoAchievementById(menkoId);
        if (!menkoAchievement.isOpen) return;

        GameObject isLockObject = newMenkoObject.transform.Find("IsLock").gameObject;
        isLockObject.SetActive(false);

        GameObject previewMenkoObject = newMenkoObject.transform.Find("Preview Menko").gameObject;
        previewMenkoObject.SetActive(true);

        UpdateRotate(previewMenkoObject, data);
        UpdateLocalScale(previewMenkoObject, data);

        GameObject upObject = FindObject(previewMenkoObject, "Up");
        GameObject downObject = FindObject(previewMenkoObject, "Down");
        GameObject sideObject = FindObject(previewMenkoObject, "Side");

        GameObject upPrefab = FindObject(MenkoPrefab, "Up");
        GameObject downPrefab = FindObject(MenkoPrefab, "Down");
        GameObject sidePrefab = FindObject(MenkoPrefab, "Side");

        GameObject updateUpObject = MenkoRank == Rank.Default ? downPrefab : upPrefab;
        GameObject updateDownPrefab = MenkoRank == Rank.Default ? upPrefab : downPrefab;

        UpdateMesh(upObject, updateUpObject, data);
        UpdateMaterials(upObject, updateUpObject, data);

        UpdateMesh(downObject, updateDownPrefab, data);
        UpdateMaterials(downObject, updateDownPrefab, data);

        UpdateMesh(sideObject, sidePrefab, data);
        UpdateMaterials(sideObject, sidePrefab, data);
    }

    private void UpdateRotate(GameObject previewMenkoObject, MenkoData data)
    {
        int MenkoId = data.GetId();
        previewMenkoObject.transform.localRotation = Quaternion.Euler(-60, 0, -40);

        if (MenkoId == 1)
        {
            previewMenkoObject.transform.localRotation = Quaternion.Euler(120, 0, -160);
        }
    }

    private void UpdateMesh(GameObject targetObject, GameObject targetPrefab, MenkoData data)
    {
        Mesh sharedMesh = targetPrefab.GetComponent<MeshFilter>().sharedMesh;
        MeshFilter targetMeshFilter = targetObject.GetComponent<MeshFilter>();

        targetMeshFilter.mesh = sharedMesh;
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

    private void UpdateLocalScale(GameObject previewMenkoObject, MenkoData data)
    {
        int MenkoId = data.GetId();

        if (MenkoId == 1)
        {
            previewMenkoObject.transform.localScale *= 0.9f;
        }
        else if (MenkoId == 3)
        {
            previewMenkoObject.transform.localScale *= 1.5f;
        }

        if (MenkoId >= 3)
        {
            float newX = previewMenkoObject.transform.localScale.x;
            float newY = MenkoId == 3 ? 3500 : 1500;
            float newZ = previewMenkoObject.transform.localScale.z;
            previewMenkoObject.transform.localScale = new Vector3(newX, newY, newZ);
        }
    }

    private GameObject FindObject(GameObject parent, string name)
    {
        return parent.transform.Find(name).gameObject;
    }
}
