using Menko.Enums;
using UnityEngine;

namespace Menko.MenkoData
{
    [System.Serializable]
    public class MenkoMesh
    {
        public static void Update(GameObject targetObject, ScriptableObjects.MenkoData data)
        {
            targetObject.tag = "Menko";

            Rank MenkoRank = data.GetRank();
            GameObject targetPrefab = data.GetPrefab();

            Rigidbody targetRigidbody = targetObject.GetComponent<Rigidbody>();
            if (targetRigidbody)
            {
                targetRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            UpdateLocalScale(targetObject, data);

            GameObject upObject = FindObject(targetObject, "Up");
            upObject.tag = "Menko";
            GameObject downObject = FindObject(targetObject, "Down");
            downObject.tag = "Menko";
            GameObject sideObject = FindObject(targetObject, "Side");
            sideObject.tag = "Menko";

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

        private static void UpdateLocalScale(GameObject targetObject, ScriptableObjects.MenkoData data)
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
            targetMeshCollider.sharedMesh = sharedMesh;
            targetMeshCollider.convex = true;
            if (targetObject.name != "Side")
            {
                targetMeshCollider.isTrigger = true;
            }
        }

        private static void UpdateMaterials(GameObject targetObject, GameObject targetPrefab, ScriptableObjects.MenkoData data)
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

    [System.Serializable]
    public enum ScanMenkoType
    {
        None,
        Up,
        Side,
        Down
    }

    [System.Serializable]
    public class ScanMenkoData
    {
        public ScanMenkoType StageMenkoType = ScanMenkoType.None;
        public ScanMenkoType UserMenkoType = ScanMenkoType.None;
    }
}
