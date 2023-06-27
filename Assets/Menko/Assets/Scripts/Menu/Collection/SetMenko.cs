using Menko.MenkoData;
using Menko.PlayerData;
using Menko.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetMenko : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    MenkoDataBase MenkoDataBase;

    [SerializeField]
    SwipeMenu SwipeMenu;

    public GameObject PreviewObject;
    private GameObject ClonePreviewObject;

    private void Start()
    {
        PlayerData playerData = Json.instance.Load();
        UpdatePreviewMenko(playerData.SetMenkoId);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        int id = SwipeMenu.SelectMenkoId + 1;

        PlayerData playerData = Json.instance.Load();
        MenkoAchievement menkoAchievement = playerData.GetMenkoAchievementById(id);
        if (!menkoAchievement.isOpen || id == playerData.SetMenkoId) return;

        playerData.UpdateMenkoSetting(id);
        Json.instance.Save(playerData);

        UpdatePreviewMenko(id);
    }

    private void UpdatePreviewMenko(int id)
    {
        if (ClonePreviewObject)
        {
            Destroy(ClonePreviewObject);
        }

        ClonePreviewObject = Instantiate(PreviewObject);
        ClonePreviewObject.transform.SetParent(transform, false);
        ClonePreviewObject.SetActive(true);

        MenkoData menkoData = MenkoDataBase.GetMenko(id);
        MenkoMesh.Update(ClonePreviewObject, menkoData);
    }
}
