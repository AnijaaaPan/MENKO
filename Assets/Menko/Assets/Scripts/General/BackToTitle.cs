using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class BackToTitle : MonoBehaviour, IPointerClickHandler
{
    public RectTransform Left;
    public RectTransform Right;

    public RectTransform OptionTransform;
    public CanvasGroup OptionCanvas;

    private float initLeftX = -1575;
    private float initRightX = 1575;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            BackScene();
        }
    }

    private async void BackScene()
    {
        for (int i = 1; i <= 50; i++)
        {
            OptionZoomOut(i * -1.5f, 1 - i * 0.05f);

            initLeftX += 30;
            initRightX -= 30;

            Left.localPosition = new Vector3(initLeftX, 100, 0);
            Right.localPosition = new Vector3(initRightX, -100, 0);

            await Task.Delay(15);
        }

        SceneManager.LoadSceneAsync("Title");
    }

    private void OptionZoomOut(float positionZ, float colorAlpha)
    {
        OptionTransform.localPosition = new Vector3(0, 0, positionZ);
        OptionCanvas.alpha = colorAlpha;
    }
}
