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

    private bool ShutOut = false;
    private float initLeftX = -2300;
    private float initRightX = 2300;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            BackScene();
        }
    }

    void Update()
    {
        if (ShutOut == true) return;
        if (!Input.GetKeyDown(KeyCode.Escape)) return ;

        BackScene();
    }

    private async void BackScene()
    {
        ShutOut = true;

        for (int i = 1; i <= 20; i++)
        {
            OptionZoomOut(i * -1.5f, 1 - i * 0.05f);

            initLeftX += 90;
            initRightX -= 90;

            Left.localPosition = new Vector3(initLeftX, 100, 0);
            Right.localPosition = new Vector3(initRightX, -100, 0);

            await Task.Delay(10);
        }

        await Task.Delay(250);
        ShutOut = false;
        SceneManager.LoadSceneAsync("Title");
    }

    private void OptionZoomOut(float positionZ, float colorAlpha)
    {
        OptionTransform.localPosition = new Vector3(0, 0, positionZ);
        OptionCanvas.alpha = colorAlpha;
    }
}
