using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UpDown : MonoBehaviour
{
    private RectTransform UpDownRectTransform;

    const float SPEED = 0.75f;
    const float HEIGHT = 4f;
    const float ROTATE = 0.15f;

    private void Start()
    {
        UpDownRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateLocalPosition();
    }

    private void UpdateLocalPosition()
    {
        float sin = Mathf.Sin(Time.time * SPEED);
        Vector3 localPosition = new(0, sin * HEIGHT, 0);
        Quaternion localRotation = Quaternion.Euler(0.0f, 0.0f, sin * ROTATE);

        UpDownRectTransform.SetLocalPositionAndRotation(localPosition, localRotation);
    }
}
