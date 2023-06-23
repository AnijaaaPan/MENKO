using Menko.Enums;
using UnityEngine;

// Menkoƒf[ƒ^
[CreateAssetMenu(fileName = "CreateMenkoData", menuName = "ScriptableObjects/CreateMenkoData")]
public class CreateMenkoData : ScriptableObject
{
    [SerializeField]
    int id;

    [SerializeField]
    Rank rank;

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    bool isAutoMosaic;

    public int GetId() => id;

    public GameObject GetPrefab() => prefab;

    public Rank GetRank() => rank;

    public bool GetIsAutoMosaic() => isAutoMosaic;
}