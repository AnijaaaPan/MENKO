using Menko.Enums;
using UnityEngine;

namespace Menko.UpdateMenko
{
    [System.Serializable]
    public class MenkoMesh
    {
        public static void Update(GameObject targetObject, MenkoData data)
        {
            Rank MenkoRank = data.GetRank();
            GameObject targetPrefab = data.GetPrefab();

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

            UpdateOutline(targetObject);
        }

        private static GameObject FindObject(GameObject parent, string name)
        {
            return parent.transform.Find(name).gameObject;
        }

        private static void UpdateLocalScale(GameObject targetObject, MenkoData data)
        {
            int MenkoId = data.GetId();
            if (MenkoId == 1)
            {
                targetObject.transform.localScale *= 0.8f;
            }
            else if (MenkoId == 3)
            {
                targetObject.transform.localScale *= 1.25f;
            }

            if (MenkoId > 3)
            {
                float newX = targetObject.transform.localScale.x;
                float newY = targetObject.transform.localScale.y * 0.725f;
                float newZ = targetObject.transform.localScale.z;
                targetObject.transform.localScale = new Vector3(newX, newY, newZ);
            }
        }

        private static void UpdateMesh(GameObject targetObject, GameObject targetPrefab)
        {
            Mesh sharedMesh = targetPrefab.GetComponent<MeshFilter>().sharedMesh;
            MeshFilter targetMeshFilter = targetObject.GetComponent<MeshFilter>();
            targetMeshFilter.mesh = sharedMesh;

            if (!targetObject.GetComponent<MeshCollider>()) return;

            MeshCollider targetMeshCollider = targetObject.GetComponent<MeshCollider>();

            if (targetObject.name != "Side")
            {
                targetMeshCollider.enabled = false;
                return;
            }
            targetMeshCollider.sharedMesh = sharedMesh;
            targetMeshCollider.convex = true;
        }

        private static void UpdateMaterials(GameObject targetObject, GameObject targetPrefab, MenkoData data)
        {
            Material[] sharedMaterials = targetPrefab.GetComponent<MeshRenderer>().sharedMaterials;
            MeshRenderer targetMeshRenderer = targetObject.GetComponent<MeshRenderer>();

            if (!data.GetIsAutoMosaic())
            {
                targetMeshRenderer.materials = sharedMaterials;
                return;
            }

            PlayerData.PlayerData playerData = Json.instance.Load();
            if (!playerData.CheckAllAchievementOpen())
            {
                targetMeshRenderer.materials = sharedMaterials;
                return;
            };

            targetMeshRenderer.material = null;
            targetMeshRenderer.material = sharedMaterials[0];
        }

        private static void UpdateOutline(GameObject targetObject)
        {
            Outline targetOutline = targetObject.GetComponent<Outline>();
            if (!targetOutline) return;

            targetOutline.enabled = false;
            targetOutline.enabled = true;
        }
    }
}
