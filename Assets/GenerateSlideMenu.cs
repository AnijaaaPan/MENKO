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
            GameObject initObject = GenerateMenkoObject(data);
            initObject.transform.SetParent(transform);
        });
    }

    private GameObject GenerateMenkoObject(MenkoData data)
    {
        GameObject newMenkoObject = Instantiate(InitMenkoObject);
        newMenkoObject.name = $"Menko {data.GetId()}";

        UpdateMenkoPreviewObject(newMenkoObject, data);
        newMenkoObject.SetActive(true);
        return newMenkoObject;
    }

    private GameObject UpdateMenkoPreviewObject(GameObject newMenkoObject, MenkoData data)
    {
        GameObject previewMenkoObject = newMenkoObject.transform.Find("Preview Menko").gameObject;
        Destroy(previewMenkoObject.GetComponent<Rigidbody>());

        Rank MenkoRank = data.GetRank();
        GameObject MenkoPrefab = data.GetPrefab();

        GameObject upObject = FindObject(newMenkoObject, "Up");
        GameObject downObject = FindObject(newMenkoObject, "Down");
        GameObject sideObject = FindObject(newMenkoObject, "Side");

        GameObject upPrefab = FindObject(MenkoPrefab, "Up");
        GameObject downPrefab = FindObject(MenkoPrefab, "Down");
        GameObject sidePrefab = FindObject(MenkoPrefab, "Side");

        GameObject updateUpObject = MenkoRank == Rank.Default ? downPrefab : upPrefab;
        GameObject updateDownPrefab = MenkoRank == Rank.Default ? upPrefab : downPrefab;

        upObject.GetComponent<MeshFilter>().mesh = updateUpObject.GetComponent<MeshFilter>().mesh;
        Destroy(upObject.GetComponent<MeshCollider>());

        downObject.GetComponent<MeshFilter>().mesh = updateDownPrefab.GetComponent<MeshFilter>().mesh;
        Destroy(upObject.GetComponent<MeshCollider>());

        sideObject.GetComponent<MeshFilter>().mesh = sidePrefab.GetComponent<MeshFilter>().mesh;
        Destroy(upObject.GetComponent<MeshCollider>());
        return newMenkoObject;
    }

    private GameObject FindObject(GameObject parent, string name)
    {
        return parent.transform.Find(name).gameObject;
    }
}
