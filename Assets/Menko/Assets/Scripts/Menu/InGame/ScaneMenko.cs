using Menko.Enums;
using Menko.GameProcess;
using Menko.MenkoData;
using Menko.ScriptableObjects;
using UnityEngine;

public class ScaneMenko : MonoBehaviour
{
    [SerializeField]
    Transform InitFieldMenkoTransform;

    public ScanMenkoData ScanMenkoData = new();

    private const float Radius = 10.5f; //ˆÚ“®o—ˆ‚é”ÍˆÍ

    void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.tag.Contains("Menko")) return;

        SetScanMenkoData(collision);
    }

    private void SetScanMenkoData(Collider collision)
    {
        GameObject parentObject = collision.gameObject.transform.parent.gameObject;
        Vector3 localPosition = parentObject.transform.localPosition;
        if (Vector3.Distance(localPosition, InitFieldMenkoTransform.localPosition) > Radius) return;

        string parentObjectName = parentObject.name;
        if (parentObjectName == "StageMenko")
        {
            ScanMenkoData.StageMenkoType = GetScanMenkoType(collision, GameProcess.instance.StageMenko);
        }
        else
        {
            BattleUserState battleUserState = GameProcess.instance.BattleUsers.Find(user =>
            {
                return user.UserType == GameProcess.instance.BattleTurn;
            });
            ScanMenkoData.UserMenkoType = GetScanMenkoType(collision, battleUserState.SetMenkoData);
        }
    }

    private ScanMenkoType GetScanMenkoType(Collider collision, MenkoData data)
    {
        Rank MenkoRank = data.GetRank();
        string objectName = collision.gameObject.name;
        if (MenkoRank == Rank.Default)
        {
            if (objectName == "Up")
            {
                return ScanMenkoType.Down;
            }
            else if (objectName == "Down")
            {
                return ScanMenkoType.Up;
            }
        }

        if (objectName == "Up")
        {
            return ScanMenkoType.Up;
        }
        else if (objectName == "Side")
        {
            return ScanMenkoType.Side;
        }
        else
        {
            return ScanMenkoType.Down;
        }
    }
}
