using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    CameraMultiTarget CameraMultiTarget;

    [SerializeField]
    GameObject[] TargetObjects;

    [SerializeField]
    TextMeshProUGUI TitleTextMeshPro;

    [SerializeField]
    TextMeshProUGUI SubTitleTextMeshPro;

    const float SPEED = 0.0035f;

    private int FadeInOut = -1; // FadeIn: 1 | FadeOut: -1

    void Start()
    {
        CameraRing.instance.InitCameraRing();
        CameraMultiTarget.SetTargets(TargetObjects);
    }

    private void Update()
    {
        UpdateFadeInOut();
        UpdateColorAlpha();
    }

    private void UpdateFadeInOut()
    {
        float colorAlpha = TitleTextMeshPro.color.a;
        if (1 <= colorAlpha)
        {
            FadeInOut = -1;
        }
        else if (colorAlpha <= 0)
        {
            FadeInOut = 1;
        }
    }

    private void UpdateColorAlpha()
    {
        Color color = TitleTextMeshPro.color;
        color.a += SPEED * FadeInOut;

        TitleTextMeshPro.color = color;
        SubTitleTextMeshPro.color = color;
    }
}
