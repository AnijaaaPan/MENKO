using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StartButton : MonoBehaviour
{
    const float SPEED = 0.002f;

    private int FadeInOut = 1; // FadeIn: 1 | FadeOut: -1
    private TextMeshProUGUI TextMeshPro;
    private Color Color;

    private void Start()
    {
        TextMeshPro = GetComponent<TextMeshProUGUI>();
        Color = TextMeshPro.color;
    }

    private void Update()
    {
        UpdateFadeInOut();
        UpdateColorAlpha();
    }

    private void UpdateFadeInOut()
    {
        if (0.25f <= Color.a)
        {
            FadeInOut = -1;
        }
        else if (Color.a <= 0)
        {
            FadeInOut = 1;
        }
    }

    private void UpdateColorAlpha()
    {
        Color.a += SPEED * FadeInOut;
        TextMeshPro.color = Color;
    }
}
