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
    private bool isCheckMenkoFlip = false;

    private async void Update()
    {
        if (!isAllMenkoSleep)
        {
            isAllMenkoSleep = CheckIsMenkoSleep();
            return;
        }

        if (!isCheckMenkoFlip)
        {
            isCheckMenkoFlip = true;
            await ScanMenko();
            // CheckAllMenkoFlip();
            return;
        }
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

    private void CheckAllMenkoFlip()
    {
        GameObject StageMenkoObject = InGameMenkoTransform.Find("StageMenko").gameObject;
        GameObject UserMenkoObject = InGameMenkoTransform.Find("UserMenko").gameObject;

    }
}
