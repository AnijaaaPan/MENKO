using System.Collections.Generic;
using Menko.Enums;
using UnityEngine;

public class GenerateSlideMenu : MonoBehaviour
{
    [SerializeField]
    GameObject InitMenkoObject;

    [SerializeField]
    MenkoDataBase MenkoDataBase;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        List<MenkoData> menkoDatas = MenkoDataBase.GetMenkos();
        menkoDatas.ForEach(data =>
        {
            GenerateMenkoObject(data);
        });
    }

    private void GenerateMenkoObject(MenkoData data)
    {
        GameObject newMenkoObject = Instantiate(InitMenkoObject);
        newMenkoObject.name = $"Menko {data.GetId()}";
        newMenkoObject.transform.SetParent(transform, false);

        GenerateMenkoPreviewObject(newMenkoObject, data);
        newMenkoObject.SetActive(true);
    }

    private void GenerateMenkoPreviewObject(GameObject newMenkoObject, MenkoData data)
    {
        Rank MenkoRank = data.GetRank();
        GameObject MenkoPrefab = data.GetPrefab();

        GameObject previewMenkoObject = newMenkoObject.transform.Find("Preview Menko").gameObject;
        UpdateLocalScale(previewMenkoObject, data);

        GameObject upObject = FindObject(previewMenkoObject, "Up");
        GameObject downObject = FindObject(previewMenkoObject, "Down");
        GameObject sideObject = FindObject(previewMenkoObject, "Side");

        GameObject upPrefab = FindObject(MenkoPrefab, "Up");
        GameObject downPrefab = FindObject(MenkoPrefab, "Down");
        GameObject sidePrefab = FindObject(MenkoPrefab, "Side");

        GameObject updateUpObject = MenkoRank == Rank.Default ? downPrefab : upPrefab;
        GameObject updateDownPrefab = MenkoRank == Rank.Default ? upPrefab : downPrefab;

        upObject.GetComponent<MeshFilter>().mesh = updateUpObject.GetComponent<MeshFilter>().sharedMesh;
        upObject.GetComponent<MeshRenderer>().materials = updateUpObject.GetComponent<MeshRenderer>().sharedMaterials;

        downObject.GetComponent<MeshFilter>().mesh = updateDownPrefab.GetComponent<MeshFilter>().sharedMesh;
        downObject.GetComponent<MeshRenderer>().materials = updateDownPrefab.GetComponent<MeshRenderer>().sharedMaterials;

        sideObject.GetComponent<MeshFilter>().mesh = sidePrefab.GetComponent<MeshFilter>().sharedMesh;
        sideObject.GetComponent<MeshRenderer>().materials = sidePrefab.GetComponent<MeshRenderer>().sharedMaterials;
    }

    private void UpdateLocalScale(GameObject previewMenkoObject, MenkoData data)
    {
        int MenkoId = data.GetId();

        if (MenkoId == 3)
        {
            previewMenkoObject.transform.localScale *= 1.5f;
        }

        if (MenkoId >= 3)
        {
            float newX = previewMenkoObject.transform.localScale.x;
            float newY = MenkoId == 3 ? 3000 : 1500;
            float newZ = previewMenkoObject.transform.localScale.z;
            previewMenkoObject.transform.localScale = new Vector3(newX, newY, newZ);
        }
    }

    private GameObject FindObject(GameObject parent, string name)
    {
        return parent.transform.Find(name).gameObject;
    }
}
