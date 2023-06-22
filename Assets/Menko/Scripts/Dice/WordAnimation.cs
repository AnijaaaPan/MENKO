using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WordAnimation : MonoBehaviour
{
    public static WordAnimation instance;

    public GameObject IsCombo;
    public GameObject IsComboButton;
    public TextMeshProUGUI IsComboText;

    public GameObject OchinchinObject;
    public GameObject OmankoObject;
    public GameObject ChinchinObject;
    public GameObject MankoObject;
    public GameObject ChinkoObject;
    public GameObject UnkoObject;
    public GameObject UnchiObject;

    private RectTransform OchinchinRectTransform;
    private TextMeshProUGUI OchinchinText;
    private TextMeshProUGUI OmankoText;
    private TextMeshProUGUI ChinchinText;
    private TextMeshProUGUI MankoText;
    private TextMeshProUGUI ChinkoText;
    private TextMeshProUGUI UnkoText;
    private TextMeshProUGUI UnchiText;

    private bool IsFirstrole = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        OchinchinRectTransform = OchinchinObject.GetComponent<RectTransform>();
        OchinchinText = OchinchinObject.GetComponent<TextMeshProUGUI>();
        ChinchinText = ChinchinObject.GetComponent<TextMeshProUGUI>();
        OmankoText = OmankoObject.GetComponent<TextMeshProUGUI>();
        MankoText = MankoObject.GetComponent<TextMeshProUGUI>();
        ChinkoText = ChinkoObject.GetComponent<TextMeshProUGUI>();
        UnkoText = UnkoObject.GetComponent<TextMeshProUGUI>();
        UnchiText = UnchiObject.GetComponent<TextMeshProUGUI>();
    }

    public async void RunAnimation()
    {
        List<RoundDiceInfo> RoundDiceInfos = GameScore.instance.RoundDiceInfos;
        RoundDiceInfos.Reverse();

        RoundDiceInfo LatestRoundInfo = RoundDiceInfos[0];

        MoveNoises.instance.VisibleAllNoise();
        if (LatestRoundInfo.DiceRole.Unchi) await AnimationText(UnchiObject, UnchiText, LatestRoundInfo.CountRoleCombo.Unchi);
        if (LatestRoundInfo.DiceRole.Unko) await AnimationText(UnkoObject, UnkoText, LatestRoundInfo.CountRoleCombo.Unko);
        if (LatestRoundInfo.DiceRole.Chinko) await AnimationText(ChinkoObject, ChinkoText, LatestRoundInfo.CountRoleCombo.Chinko);
        if (LatestRoundInfo.DiceRole.Manko) await AnimationText(MankoObject, MankoText, LatestRoundInfo.CountRoleCombo.Manko);
        if (LatestRoundInfo.DiceRole.Omanko) await AnimationText(OmankoObject, OmankoText, LatestRoundInfo.CountRoleCombo.Omanko);
        if (LatestRoundInfo.DiceRole.Chinchin) await AnimationText(ChinchinObject, ChinchinText, LatestRoundInfo.CountRoleCombo.Chinchin);
        if (LatestRoundInfo.DiceRole.Ochinchin) await AnimationText(OchinchinObject, OchinchinText, LatestRoundInfo.CountRoleCombo.Ochinchin, true);
        MoveNoises.instance.VisibleAllNoise(false);

        GameStart.instance.WaitType = 5;
        Sound.instance.SoundHyoushigi();
    }

    private async Task IsOchinchinAnime(TextMeshProUGUI TextMeshProUGUI)
    {
        TextMeshProUGUI.color = Color.black;
        IsCombo.SetActive(true);
        IsComboButton.SetActive(false);

        float InitXPos = 3250;
        for (int i = 0; i < 200; i++)
        {
            InitXPos -= 40f;
            if (InitXPos <= -3250) break;

            OchinchinRectTransform.localPosition = new Vector3(InitXPos, 0, 0);
            await Task.Delay(10);
        }

        OchinchinRectTransform.localPosition = new Vector3(0, 0, 0);

        TextMeshProUGUI.color = Color.white;
        IsCombo.SetActive(false);
    }

    private async Task ComboZoomOut(TextMeshProUGUI TextMeshProUGUI, int Combo, bool IsOchinchin = false)
    {
        bool IsChange = false;
        IsCombo.SetActive(Combo > 1);
        if (IsOchinchin == false)
        {
            IsComboButton.SetActive(Combo > 1);
        }

        TextMeshProUGUI.color = Combo > 1 ? Color.black : Color.white;
        TextMeshProUGUI.fontSize = 600;

        string ComboText = $"COMBO {Combo}";
        for (int i = 0; i < 60; i++)
        {
            string randomText = RandomPassword.Generate(ComboText.Length);
            IsComboText.SetText(i <= 5 ? randomText : ComboText);
            if (TextMeshProUGUI != null) TextMeshProUGUI.fontSize -= 5;
            if (TextMeshProUGUI.fontSize <= 400 && IsChange == false)
            {
                IsChange = true;
                TextMeshProUGUI.color = Color.white;
                IsCombo.SetActive(false);
            }
            await Task.Delay(20);
        }
    }

    public async Task AnimationText(GameObject AnimeObject, TextMeshProUGUI Text, int Combo, bool IsOchinchin = false)
    {
        AnimeObject.SetActive(true);
        if (IsFirstrole == true)
        {
            IsFirstrole = false;
            GameStart.instance.UpdateRemainRollCount(1);
        }

        GameStart.instance.DiceCount++;
        GameStart.instance.UpdateRemainNudgeCount(1);
        if (IsOchinchin == true)
        {
            Sound.instance.SoundOchinchin();
            await IsOchinchinAnime(Text);
        }
        else
        {
            Sound.instance.SoundIyoh();
        }

        await ComboZoomOut(Text, Combo, IsOchinchin);

        await Task.Delay(2000);

        AnimeObject.SetActive(false);
        await Task.Delay(250);
    }
}
