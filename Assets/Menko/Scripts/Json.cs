using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Quality; // 1: Low, 2:Mid, 3: High
    public int Speed; // 1: 0.5倍速, 2: 0.75倍速, 3: 1倍速, 4: 1.25倍速, 5: 1.5倍速
    public int BestScore;
    public int WorstScore;
    public int Ochinchin;
}

public class Json : MonoBehaviour
{
    public static Json instance;
    string datapath;

    private void Awake()
    {
        datapath = Application.dataPath + "/Json/Player.json";

        if (instance == null)
        {
            instance = this;
        }
    }

    private void InitJsonFile()
    {
        string directoryPath = Application.dataPath + "/Json";
        if (Directory.Exists(directoryPath)) return;
        Directory.CreateDirectory(directoryPath);

        if (File.Exists(datapath)) return;
        FileStream fs = File.Create(datapath);
        fs.Close();

        PlayerData player = new()
        {
            Quality = 1,
            Speed = 3,
            BestScore = 0,
            WorstScore = 0,
            Ochinchin = 0
        };
        Save(player);
    }

    public void Start()
    {
        InitJsonFile();

        PlayerData player = Load();
        UpdateResolution(player.Quality);
        UpdateTimeScale(player.Speed);
        Save(player);
    }

    public void UpdateTimeScale(int SpeedType)
    {
        if (SpeedType == 1)
        {
            Time.timeScale = 0.5f;
        }
        else if (SpeedType == 2)
        {
            Time.timeScale = 0.75f;
        }
        else if (SpeedType == 3)
        {
            Time.timeScale = 1f;
        }
        else if (SpeedType == 4)
        {
            Time.timeScale = 1.25f;
        }
        else
        {
            Time.timeScale = 1.5f;
        }
    }

    private void UpdateResolution(int QualityType)
    {
        if (QualityType == 1)
        {
            Screen.SetResolution(1920, 1080, true, 60);
        }
        else if (QualityType == 2)
        {
            Screen.SetResolution(2560, 1440, true, 60);
        }
        else
        {
            Screen.SetResolution(3840, 2160, true, 60);
        }
    }

    public PlayerData Load()
    {
        FileStream fs = new(datapath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamReader reader = new(fs);
        string datastr = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<PlayerData>(datastr);
    }

    public void Save(PlayerData player)
    {
        string jsonstr = JsonUtility.ToJson(player, true);
        FileStream fs = new(datapath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter writer = new(fs);
        writer.WriteLine(jsonstr);
        writer.Flush();
        writer.Close();
    }
}