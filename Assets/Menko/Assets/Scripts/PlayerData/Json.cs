using Menko.PlayerData;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Json : MonoBehaviour
{
    public static Json instance;
    string datapath;

    public MenkoDataBase MenkoDataBase;

    private void Awake()
    {
        datapath = Application.dataPath + "/Json/Player.json";

        InitJsonFile();

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

        List<MenkoData> MenkoDatas = MenkoDataBase.GetMenkos();
        PlayerData player = PlayerData.Init(MenkoDatas);
        Save(player);
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