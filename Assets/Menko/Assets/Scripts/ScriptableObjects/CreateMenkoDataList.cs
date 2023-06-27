using System.Collections.Generic;
using UnityEngine;

namespace Menko.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CreateMenkoDataBase", menuName = "ScriptableObjects/CreateMenkoDataBase")]
    public class MenkoDataBase : ScriptableObject
    {
        [SerializeField]
        private List<MenkoData> menkoDatas = new();

        public List<MenkoData> GetMenkos()
        {
            return menkoDatas;
        }

        public MenkoData GetMenko(int id)
        {
            return menkoDatas.Find(Menko => Menko.GetId() == id);
        }
    }
}