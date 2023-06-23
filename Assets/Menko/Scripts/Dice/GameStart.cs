using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public static GameStart instance;

    public CameraFilter MonoFilter;
    public CameraMultiTarget cameraMultiTarget;
    public GameObject[] BowlObject;
    public GameObject[] TitleObjects;
    public GameObject InGameUIObject;
    public Rigidbody BowlRigidbody;

    public CanvasGroup BowlCanvasGroup;
    public CanvasGroup NextRoundCanvasGroup;
    public CanvasGroup GameResultCanvasGroup;
    public CanvasGroup BackToTitleCanvasGroup;
    public TextMeshProUGUI RollText;
    public TextMeshProUGUI DiceCountText;
    public TextMeshProUGUI RemainRollText;
    public TextMeshProUGUI RemainNudgeText;
    public Cinemachine.CinemachineImpulseSource CinemachineImpulseSource;

    public CameraDice CameraDice;
    // public CameraBowl CameraBowl;

    public int Round = 1;
    public int DiceCount = 5;
    public int WaitType = 0; // 0: 最初のアニメーション中, 1: クリック待機中, 2: サイコロ落下中, 3: サイコロ落下後のNUDGE受付時間, 4: 役などを確認, 4: ゲームを次に進める
    public int RemainRound = 3;
    public int RemainNudge = 5;
    public bool IsClick = false;

    public int GameType;
    private float time;
    private float NextRoundTime;
    private bool IsFirst = false;
    private bool IsNudgeClick = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private async void Update()
    {
        if (WaitType == 0 || IsClick == true) return;

        if (WaitType == 1 && Input.GetMouseButton(0))
        {
            Sound.instance.UpdateBGM(true);
            UpdateRemainRollCount(0);
            WaitType = 2;
            IsClick = true;

            GameProcess.instance.InitSetDice();
            CameraDice.enabled = true;
            // CameraBowl.enabled = false;
            CameraDice.instance.InitCameraDice();
            RandomTimeScale();
        }

        if (WaitType == 3 && RemainNudge != 0 && IsNudgeClick == false && Input.GetMouseButton(1))
        {
            IsClick = true;
            IsFirst = true;
            IsNudgeClick = true;
            GameProcess.instance.IsSleepingDices = new List<GameObject>();
            GameProcess.instance.AllDiceSleep = 0;
            Sound.instance.SoundNudge();
            CinemachineImpulseSource.GenerateImpulse();
            UpdateRemainNudgeCount(0);
        }

        if (WaitType == 3 && IsNudgeClick == true && Input.GetMouseButtonUp(1))
        {
            IsNudgeClick = false;
        }

        if (WaitType == 5)
        {
            NextRoundTime += Time.unscaledDeltaTime * 3.5f;
            NextRoundCanvasGroup.alpha = Mathf.Sin(NextRoundTime) + 1f;

            if (RemainRound != 0 && Input.GetMouseButton(0))
            {
                WaitType = 0;
                NextRoundCanvasGroup.alpha = 0;
                await FadeInOutImage.instance.FadeInOut(false, 0.055f, 10);
                InitGame(GameType, true);
            }
            else if (RemainRound == 0 && Input.GetMouseButton(0))
            {
                IsClick = true;
                WaitType = 6;
                InGameUIObject.SetActive(false);

                await FadeInOutImage.instance.FadeInOut(false, 0.055f, 10);
                for (int i = 1; i <= 100; i++)
                {
                    GameResultCanvasGroup.alpha = i * 0.01f;
                    await Task.Delay(10);
                }
                RemainRollText.alpha = 1;
                IsClick = false;
            }
        }


        if (WaitType == 6)
        {
            NextRoundTime += Time.unscaledDeltaTime * 3.5f;
            BackToTitleCanvasGroup.alpha = Mathf.Sin(NextRoundTime) + 1f;

            if (Input.GetMouseButton(0))
            {
                SceneManager.LoadSceneAsync("Title");
            }
        }
    }

    void FixedUpdate()
    {
        if (IsFirst)
        {
            IsFirst = false;
            Vector3 InitPos = BowlObject[0].transform.position;
            Vector3 ImpulseVec = new Vector3(Random.Range(0, 0.05f), Random.Range(0.075f, 0.125f), Random.Range(0, 0.05f));
            BowlRigidbody.MovePosition(InitPos + ImpulseVec);
            IsClick = false;
        }
    }

    public async void InitGame(int Type, bool IsNextRound = false)
    {
        if (IsNextRound == true)
        {
            Round++;
            GameProcess.instance.IsDroping = true;
            GameProcess.instance.DeleteDices();
            GameProcess.instance.IsSleepingDices = new List<GameObject>();
            GameScore.instance.DiceValues = new List<DiceValue>();
            ScaneDice.instance.IsScaning = false;
            GameProcess.instance.AllDiceSleep = 0;
            IsClick = false;
            time = 0;
            NextRoundTime = 0;
            IsFirst = false;
            IsNudgeClick = false;
        }

        Sound.instance.UpdateBGM(false);
        GameType = Type;
        TitleObjects[GameType - 1].SetActive(true);
        await FadeInOutImage.instance.FadeInOut(false, 0.005f, 25);
        GameProcess.instance.DeleteDices();

        CameraDice.enabled = false;
        MonoFilter.enabled = false;
        // CameraBowl.enabled = true;
        // CameraBowl.instance.InitCameraBowl();
        GameProcess.instance.InitSetCameraObject(BowlObject);

        RollText.text = $"ROLL : {Round}";
        await FadeInOutImage.instance.FadeInOut(true, 0.055f, 20);
        UpdateBowlCanvas();
    }

    private async void UpdateBowlCanvas()
    {
        string DiceText = $"{DiceCount} DICE";
        for (int i = 1; i <= 15; i++)
        {
            BowlCanvasGroup.alpha = i * 0.1f;

            // string randomText = RandomPassword.Generate(DiceText.Length);
            // DiceCountText.text = randomText;
            await Task.Delay(20);
        }

        DiceCountText.text = DiceText;

        await Task.Delay(1000);

        for (int i = 0; i < 20; i++)
        {
            BowlCanvasGroup.alpha = GetAlphaColor();
            await Task.Delay(6);
        }
        BowlCanvasGroup.alpha = 0;

        await Task.Delay(750);
        InGameUIObject.SetActive(true);
        WaitType = 1;

        CanvasGroup UICanvasGroup = InGameUIObject.GetComponent<CanvasGroup>();
        for (int i = 1; i <= 10; i++)
        {
            UICanvasGroup.alpha = i * 0.1f;
            await Task.Delay(50);
        }
    }

    private float GetAlphaColor()
    {
        time += Time.unscaledDeltaTime * 20;
        return Mathf.Sin(time) * 0.5f + 0.5f;
    }

    public async void UpdateRemainRollCount(int UpdateType = 0)
    {
        RemainRollText.text = RemainRound.ToString();

        if (UpdateType == 0)
        {
            RemainRound -= 1;
            for (int i = 1; i <= 10; i++)
            {
                RemainRollText.alpha = i % 2 == 0 ? 1 : 0;
                await Task.Delay(25);
            }
            RemainRollText.alpha = 1;

        }
        else if (UpdateType == 1)
        {
            RemainRound += 1;
            RemainRollText.text = "+1";
            await Task.Delay(500);
        }

        RemainRollText.text = RemainRound.ToString();
    }

    public async void UpdateRemainNudgeCount(int UpdateType = 0)
    {
        RemainNudgeText.text = $"NUDGE   {RemainNudge}";

        if (UpdateType == 0)
        {
            RemainNudge -= 1;
        }
        else if (UpdateType == 1)
        {
            RemainNudge += 1;
            RemainNudgeText.text = "NUDGE   +1";
            await Task.Delay(500);
        }

        RemainNudgeText.text = $"NUDGE   {RemainNudge}";

        float ColorValue = RemainNudge == 0 ? 0.25f : 1;
        RemainNudgeText.color = new Color(ColorValue, ColorValue, ColorValue);
    }

    private async void RandomTimeScale()
    {
        PlayerData PlayerData = Json.instance.Load();
        await Task.Delay(Random.Range(200, 400));

        float timeScale = Random.Range(0.1f, 0.5f);
        Time.timeScale *= timeScale;

        Sound.instance.SoundFaa();
        await Task.Delay(Random.Range(1500, 2000));
        Sound.instance.SoundEffect.Stop();

        Json.instance.UpdateTimeScale(PlayerData.Speed);
    }
}
