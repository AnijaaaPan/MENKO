using UnityEngine;
using System.Threading.Tasks;

public class ActiveOption : MonoBehaviour
{
    public RectTransform OptionTransform;
    public CanvasGroup OptionCanvas;
    public GameObject ContentObject;
    public GameObject SetMenko;

    async void Start()
    {
        await Task.Delay(500);

        for (int i = 1; i <= 10; i++)
        {
            OptionZoomOut(-30 + i * 3, i * 0.1f);
            await Task.Delay(20);
        }

        ContentObject.SetActive(true);
        SetMenko.SetActive(true);
    }

    private void OptionZoomOut(int positionZ, float colorAlpha)
    {
        OptionTransform.localPosition = new Vector3(0, 0, positionZ);
        OptionCanvas.alpha = colorAlpha;
    }
}
