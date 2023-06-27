using Menko.Enums;
using UnityEngine;

namespace Menko.ScriptableObjects
{
    // Menkoƒf[ƒ^
    [CreateAssetMenu(fileName = "CreateMenkoData", menuName = "ScriptableObjects/CreateMenkoData")]
    public class MenkoData : ScriptableObject
    {
        [SerializeField]
        int id;

        [SerializeField]
        Rank rank;

        [SerializeField]
        GameObject prefab;

        [SerializeField]
        Material material;

        [SerializeField]
        bool isAutoMosaic;

        public int GetId() => id;

        public GameObject GetPrefab() => prefab;

        public Material GetMaterial() => material;

        public Rank GetRank() => rank;

        public bool GetIsAutoMosaic() => isAutoMosaic;
    }

}