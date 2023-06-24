using Menko.PlayerData;
using Menko.Enums;
using UnityEngine;

public class SetMenko : MonoBehaviour
{
    [SerializeField]
    MenkoDataBase MenkoDataBase;

    public GameObject MainPreviewObject;
    public GameObject SubPreviewObject;

    private void Start()
    {
        UpdatePreviewMenko(Setting.Main);
        UpdatePreviewMenko(Setting.Sub);
    }

    public void UpdatePreviewMenko(Setting setId)
    {
        PlayerData playerData = Json.instance.Load();
        MenkoSetting menkoSetting = playerData.GetMenkoSettingByIndex(setId);
        MenkoData menkoData = MenkoDataBase.GetMenko(menkoSetting.id);
        UpdatePreviewObject(menkoData, setId);
    }

    private GameObject FindObject(GameObject parent, string name)
    {
        return parent.transform.Find(name).gameObject;
    }

    private void UpdatePreviewObject(MenkoData data, Setting setId)
    {
        Rank MenkoRank = data.GetRank();
        GameObject MenkoPrefab = data.GetPrefab();

        GameObject previewMenkoObject = setId == Setting.Main ? MainPreviewObject : SubPreviewObject;
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

    private void UpdateRotate(GameObject previewMenkoObject, MenkoData data)
    {
        int MenkoId = data.GetId();
        previewMenkoObject.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        if (MenkoId == 1)
        {
            previewMenkoObject.transform.localRotation = Quaternion.Euler(90, 180, 0);
        }
    }

    private void UpdateLocalScale(GameObject previewMenkoObject, MenkoData data)
    {
        int MenkoId = data.GetId();

        previewMenkoObject.transform.localScale = new Vector3(75, 75, 75);
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
}
