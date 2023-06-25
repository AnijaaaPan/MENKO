using UnityEngine;

public class LookToMainCamera : MonoBehaviour
{
    public GameObject CameraObject;

    private RectTransform RectTransform;

    private void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 localAngle = new(0, -180, GetAngle());
        RectTransform.localEulerAngles = localAngle;
    }

    float GetAngle()
    {
        Vector3 dt = transform.position - CameraObject.transform.position;
        float rad = Mathf.Atan2(dt.z, dt.x);
        float degree = rad * Mathf.Rad2Deg;
        return degree + 90;
    }
}
