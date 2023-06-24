using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DiceValue
{
    public GameObject DiceObject;
    public string Word;
}

[System.Serializable]
public class DiceRole
{
    public bool Ochinchin = false;
    public bool Omanko = false;
    public bool Chinchin = false;
    public bool Chinko = false;
    public bool Manko = false;
    public bool Unchi = false;
    public bool Unko = false;
}

[System.Serializable]
public class CountDiceWord
{
    public int O;
    public int C;
    public int N;
    public int U;
    public int M;
    public int K;
}

[System.Serializable]
public class CountRoleCombo
{
    public int Ochinchin;
    public int Omanko;
    public int Chinchin;
    public int Chinko;
    public int Manko;
    public int Unchi;
    public int Unko;
}

[System.Serializable]
public class RoundDiceInfo
{
    public int Round;
    public DiceRole DiceRole;
    public CountDiceWord CountDiceWord;
    public CountRoleCombo CountRoleCombo;
}

public class GameScore : MonoBehaviour
{
    public static GameScore instance;

    public int AllScore;
    public int UScore;
    public int MScore;
    public int CScore;

    public List<DiceValue> DiceValues = new List<DiceValue>();
    public List<RoundDiceInfo> RoundDiceInfos = new List<RoundDiceInfo>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddDiceWord(GameObject Object, string Word)
    {
        DiceValue DiceValue = new DiceValue()
        {
            DiceObject = Object,
            Word = Word,
        };
        DiceValues.Add(DiceValue);

        if (DiceValues.Count == GameProcess.instance.IsSleepingDices.Count) EndGameRound();
    }

    private void EndGameRound()
    {
        RoundDiceInfo RoundDiceInfo = new RoundDiceInfo();
        RoundDiceInfo.Round = GameStart.instance.Round;

        CountDiceWord CountDiceWord = new CountDiceWord();
        for (int i = 0; i < DiceValues.Count; i++)
        {
            DiceValue DiceValue = DiceValues[i];

            if (DiceValue.Word == "O") CountDiceWord.O++;
            if (DiceValue.Word == "C") CountDiceWord.C++;
            if (DiceValue.Word == "N") CountDiceWord.N++;
            if (DiceValue.Word == "U") CountDiceWord.U++;
            if (DiceValue.Word == "M") CountDiceWord.M++;
            if (DiceValue.Word == "K") CountDiceWord.K++;
        }
        RoundDiceInfo.CountDiceWord = CountDiceWord;

        DiceRole DiceRole = new DiceRole();
        CountRoleCombo CountRoleCombo = new CountRoleCombo();

        bool IsOchinchin = CountDiceWord.O >= 1 && CountDiceWord.C >= 2 && CountDiceWord.N >= 2;
        DiceRole.Ochinchin = IsOchinchin;
        CountRoleCombo.Ochinchin = IsOchinchin == true ? CountRoleCombo.Ochinchin + 1 : 0;

        bool IsOmanko = CountDiceWord.O >= 1 && CountDiceWord.M >= 1 && CountDiceWord.N >= 1 && CountDiceWord.K >= 1;
        DiceRole.Omanko = IsOmanko;
        CountRoleCombo.Omanko = IsOmanko == true ? CountRoleCombo.Omanko + 1 : 0;

        bool IsChinchin = CountDiceWord.C >= 2 && CountDiceWord.N >= 2;
        DiceRole.Chinchin = IsChinchin;
        CountRoleCombo.Chinchin = IsChinchin == true ? CountRoleCombo.Chinchin + 1 : 0;

        bool IsChinko = CountDiceWord.C >= 1 && CountDiceWord.N >= 1 && CountDiceWord.K >= 1;
        DiceRole.Chinko = IsChinko;
        CountRoleCombo.Chinko = IsChinko == true ? CountRoleCombo.Chinko + 1 : 0;

        bool IsManko = CountDiceWord.M >= 1 && CountDiceWord.N >= 1 && CountDiceWord.K >= 1;
        DiceRole.Manko = IsManko;
        CountRoleCombo.Manko = IsManko == true ? CountRoleCombo.Manko + 1 : 0;

        bool IsUnko = CountDiceWord.U >= 1 && CountDiceWord.N >= 1 && CountDiceWord.K >= 1;
        DiceRole.Unko = IsUnko;
        CountRoleCombo.Unko = IsUnko == true ? CountRoleCombo.Unko + 1 : 0;

        bool IsUnchi = CountDiceWord.U >= 1 && CountDiceWord.N >= 1 && CountDiceWord.C >= 1;
        DiceRole.Unchi = IsUnchi;
        CountRoleCombo.Unchi = IsUnchi == true ? CountRoleCombo.Unchi + 1 : 0;

        RoundDiceInfo.DiceRole = DiceRole;
        RoundDiceInfo.CountRoleCombo = CountRoleCombo;
        RoundDiceInfos.Add(RoundDiceInfo);

    }

    private void AddScoreByWord(string Word)
    {

    }

    private void AddScoreByRole()
    {

    }

    private void AddScoreByDoublets()
    {

    }
}
