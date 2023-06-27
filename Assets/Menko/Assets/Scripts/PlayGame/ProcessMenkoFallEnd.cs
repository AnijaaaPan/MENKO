using Menko.GameProcess;
using Menko.MenkoData;
using System.Threading.Tasks;
using UnityEngine;

public class ProcessMenkoFallEnd : MonoBehaviour
{
    [SerializeField]
    Transform InGameMenkoTransform;

    [SerializeField]
    GameObject[] TargetObjects;

    [SerializeField]
    GameObject ScanPanelObject;

    private bool isAllMenkoSleep = false;

    private async void Update()
    {
        if (isAllMenkoSleep) return;

        isAllMenkoSleep = CheckIsMenkoSleep();
        if (!isAllMenkoSleep) return;

        ScanMenkoData ScanMenkoData = await ScanMenko();
        EndGameOrNextRound(ScanMenkoData);
    }

    public void Run()
    {
        Time.timeScale = 1;

        GameProcess.instance.EnableCameraRing();
        GameProcess.instance.InitSetCameraObject(TargetObjects);
        CameraRing.instance.ResetCameraMove();
        CameraRing.instance.ReStart();
    }

    private bool CheckIsMenkoSleep()
    {
        for (int i = 0; i < InGameMenkoTransform.childCount; i++)
        {
            GameObject MenkoObject = InGameMenkoTransform.GetChild(i).gameObject;
            if (MenkoObject.activeSelf == false) continue;

            Rigidbody MenkoRigidbody = MenkoObject.GetComponent<Rigidbody>();
            if (!MenkoRigidbody.IsSleeping()) return false;
        }
        return true;
    }

    private async Task<ScanMenkoData> ScanMenko()
    {
        ScanPanelObject.SetActive(true);

        for (int i = 0; i <= 50; i++)
        {
            ScanPanelObject.transform.localPosition = new Vector3(0, 0, 0.04f * i);
            await Task.Delay(10);
        }

        ScaneMenko scaneMenko = ScanPanelObject.GetComponent<ScaneMenko>();
        return scaneMenko.ScanMenkoData;
    }

    private void EndGameOrNextRound(ScanMenkoData ScanMenkoData)
    {
        bool isStageMenkoFlip = ScanMenkoData.StageMenkoType == ScanMenkoType.Down;
        bool isStageMenkoNone = ScanMenkoData.StageMenkoType == ScanMenkoType.None;
        bool isUserMenkoFlip = ScanMenkoData.UserMenkoType == ScanMenkoType.Down;
        bool isUserMenkoNone = ScanMenkoData.UserMenkoType == ScanMenkoType.None;

        if (IsResetNextRound(isStageMenkoFlip, isStageMenkoNone, isUserMenkoFlip, isUserMenkoNone))
        {
            GameProcess.instance.UpdateProcessNextRound(true);
            return;
        }

        BattleUserType? WinUserType = GetWinUserType(isStageMenkoFlip, isStageMenkoNone, isUserMenkoFlip, isUserMenkoNone);
        if (WinUserType != null)
        {
            GameProcess.instance.UpdateProcessEndGame(WinUserType.Value);
            return;
        }

        GameProcess.instance.UpdateProcessNextRound();
    }

    private bool IsResetNextRound(bool isStageFlip, bool isStageNone, bool isUserFlip, bool isUserNone)
    {
        if (isStageFlip && isUserFlip) return true;
        if (isStageNone && isUserNone) return true;
        if (isStageFlip && isUserNone) return true;
        if (isStageNone && isUserFlip) return true;
        return false;
    }

    private BattleUserType? GetWinUserType(bool isStageFlip, bool isStageNone, bool isUserFlip, bool isUserNone)
    {
        if (isUserFlip || isUserNone)
        {
            return BattleUserType.Stage;
        }
        else if (isStageFlip || isStageNone)
        {
            return GameProcess.instance.BattleTurn;
        }
        else
        {
            return null;
        }
    }
}
