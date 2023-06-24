using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    private void Update()
    {
        float rotateX = transform.localEulerAngles.x;
        float rotateY = transform.localEulerAngles.y - 0.25f;
        float rotateZ = transform.localEulerAngles.z;
        transform.localRotation = Quaternion.Euler(rotateX, rotateY, rotateZ);
    }
}
