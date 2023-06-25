using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnClickMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject StartObject;

    [SerializeField]
    GameObject CollectionObject;

    [SerializeField]
    GameObject StartMenu;

    [SerializeField]
    GameObject CollectionMenuObject;

    [SerializeField]
    CanvasGroup StartMenuCanvas;

    [SerializeField]
    GameObject InGameObject;

    [SerializeField]
    GameObject FadeInOutImageObject;

    private float time;
    private Image ObjectImage;
    private RectTransform StartMenuRectTransform;

    private void Start()
    {
        ObjectImage = GetComponent<Image>();
        StartMenuRectTransform = StartMenu.GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ObjectFlashing(eventData.pointerEnter);
        }
    }

    private async void ObjectFlashing(GameObject Object)
    {
        for (int i = 0; i < 20; i++)
        {
            ObjectImage.color = GetAlphaColor(ObjectImage.color);
            await Task.Delay(6);
        }

        ObjectImage.color = new Color(0, 0, 0, 0);

        ZoomIn(-10, 0.9f);
        await Task.Delay(10);
        ZoomIn(-20, 0.8f);
        await Task.Delay(10);
        ZoomIn(-30, 0.7f);
        await Task.Delay(10);
        ZoomIn(-40, 0.6f);
        await Task.Delay(10);
        ZoomIn(-60, 0.4f);
        await Task.Delay(10);
        ZoomIn(-80, 0.2f);
        await Task.Delay(10);
        ZoomIn(-100, 0.1f);
        await Task.Delay(10);
        ZoomIn(-150, 0f);

        StartMenu.SetActive(false);

        ObjectFunc(Object);
    }

    private Color GetAlphaColor(Color color)
    {
        time += Time.unscaledDeltaTime * 20;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;
        return color;
    }

    private void ZoomIn(int positionZ, float colorAlpha)
    {
        StartMenuRectTransform.localPosition = new Vector3(0, 0, positionZ);
        StartMenuCanvas.alpha = colorAlpha;
    }

    private void ObjectFunc(GameObject Object)
    {
        if (Object == CollectionObject)
        {
            CollectionMenuObject.SetActive(true);
            return;
        }

        if (Object == StartObject)
        {
            InGameObject.SetActive(true);
            FadeInOutImageObject.SetActive(false);
            gameObject.SetActive(false);
            return;
        }
    }
}
