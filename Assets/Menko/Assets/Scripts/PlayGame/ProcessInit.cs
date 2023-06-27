using Menko.MenkoData;
using Menko.ScriptableObjects;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ProcessInit : MonoBehaviour
{
    [SerializeField]
    GameObject RingObject;

    [SerializeField]
    GameObject TitleMenkoObjects;

    [SerializeField]
    GameObject InGameMenkoObjects;

    [SerializeField]
    GameObject InitMenkoObject;

    [SerializeField]
    CameraRing CameraRing;

    [SerializeField]
    GameObject BattleReadyCanvasObject;

    [SerializeField]
    Transform InitFieldMenkoTransform;

    private float time;

    private void Start()
    {

    }

    public async void Run()
    {
        FadeInOutImage.instance.isTitle = false;
        await FadeInOutImage.instance.FadeInOut(false, 0.01f, 15);

        TitleMenkoObjects.SetActive(false);
        InGameMenkoObjects.SetActive(true);
        SetBattleMenko();

        await FadeInOutImage.instance.FadeInOut(true, 0.05f, 20);
        await ShowBattleReadyCanvas();

        GameProcess.instance.UpdateProcessWaitStart();
    }

    private async Task ShowBattleReadyCanvas()
    {
        string initText = "Ready";
        CanvasGroup CanvasGroup = BattleReadyCanvasObject.GetComponent<CanvasGroup>();
        TextMeshProUGUI BattleReadyText = BattleReadyCanvasObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        for (int i = 1; i <= 15; i++)
        {
            CanvasGroup.alpha = i * 0.1f;

            string randomText = RandomPassword.Generate(initText.Length);
            BattleReadyText.text = randomText;
            await Task.Delay(15);
        }

        for (int i = 0; i <= 3; i++)
        {
            string addText = new('.', i);
            BattleReadyText.text = $"{initText}{addText}";
            await Task.Delay(500);
        }

        BattleReadyText.text = "Fight";
        for (int i = 0; i < 40; i++)
        {
            CanvasGroup.alpha = GetAlphaColor();
            await Task.Delay(15);
        }
        CanvasGroup.alpha = 0;
    }

    private float GetAlphaColor()
    {
        time += Time.unscaledDeltaTime * 20;
        return Mathf.Sin(time) * 0.5f + 0.5f;
    }

    private void SetBattleMenko()
    {
        MenkoData stageMenkoData = GameProcess.instance.GetRandomMenkoObject(true);
        GameProcess.instance.StageMenko = stageMenkoData;

        Vector3 InitPos = InitMenkoObject.transform.localPosition;
        Quaternion InitQ = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);

        GameObject MenkoPrefab = stageMenkoData.GetPrefab();
        GameObject MenkoObject = Instantiate(MenkoPrefab, InitPos, InitQ);
        MenkoObject.name = "StageMenko";
        MenkoMesh.Update(MenkoObject, stageMenkoData);

        Outline MenkoOutline = MenkoObject.AddComponent<Outline>();
        MenkoOutline.OutlineMode = Outline.Mode.OutlineVisible;
        MenkoOutline.OutlineColor = new(1, 1, 1, 0.75f);
        MenkoOutline.OutlineWidth = 2.5f;

        UpdateMenkoLayer UpdateMenkoLayer = MenkoObject.AddComponent<UpdateMenkoLayer>();
        UpdateMenkoLayer.InitFieldMenkoTransform = InitFieldMenkoTransform;

        MenkoObject.SetActive(true);
        MenkoObject.transform.SetParent(InGameMenkoObjects.transform, false);
    }
}
