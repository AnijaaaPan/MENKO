using Menko.GameProcess;
using Menko.MenkoData;
using Menko.PlayerData;
using Menko.ScriptableObjects;
using UnityEngine;

public class ProcessEndGame : MonoBehaviour
{
    [SerializeField]
    GameObject InGameUIObject;

    [SerializeField]
    GameObject EndGameUIObject;

    [SerializeField]
    GameObject WinUIObject;

    [SerializeField]
    GameObject WinMenkoPreview;

    [SerializeField]
    GameObject LoseUIObject;

    private void Start()
    {

    }

    public async void Run(BattleUserType WinUserType)
    {
        await FadeInOutImage.instance.FadeInOut(false, 0.05f, 20);
        InGameUIObject.SetActive(false);
        EndGameUIObject.SetActive(true);

        if (WinUserType != BattleUserType.Player)
        {
            PlayerLose();
            return;
        }

        PlayerWin();
    }

    private void PlayerWin()
    {
        MenkoData data = GameProcess.instance.StageMenko;
        MenkoMesh.Update(WinMenkoPreview, data);
        WinUIObject.SetActive(true);

        PlayerData playerData = Json.instance.Load();
        playerData.UpdateMenkoAchievement(data.GetId(), true);
        Json.instance.Save(playerData);
    }

    private void PlayerLose()
    {
        LoseUIObject.SetActive(true);
    }
}
