using UnityEngine;
using UnityEngine.EventSystems;
using Menko.PlayerData;

public class UpdateSetMenko : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Setting Setting;

    [SerializeField]
    SetMenko SetMenko;

    [SerializeField]
    SwipeMenu SwipeMenu;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        int id = SwipeMenu.SelectMenkoId + 1;

        PlayerData playerData = Json.instance.Load();
        MenkoAchievement menkoAchievement = playerData.GetMenkoAchievementById(id);
        if (!menkoAchievement.isOpen) return;

        playerData.UpdateMenkoSetting(Setting, SwipeMenu.SelectMenkoId + 1);
        Json.instance.Save(playerData);

        SetMenko.UpdatePreviewMenko(Setting);
    }

}