using UnityEngine;
using UnityEngine.UI;

public class GlitchImage : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        UpdateColor();
        UpdateRotate();
    }

    private void UpdateColor()
    {
        Color imageColor = image.color;
        imageColor.a = 1 - Mathf.PingPong(Time.time / 6, 0.25f);
        image.color = imageColor;
    }

    private void UpdateRotate()
    {
        float rotateionZ = -7.5f + Mathf.PingPong(Time.time * 2, 15f);
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, rotateionZ);
    }
}
