using UnityEngine;

public class PointerAnimation : MonoBehaviour
{
    public float nowPosZ;

    private Rigidbody rb;

    private void Start()
    {
        nowPosZ = transform.localPosition.z;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, nowPosZ - Mathf.PingPong(Time.time / 300, 0.0075f));

        Quaternion addRotation = Quaternion.Euler(0, 5f, 0f);
        rb.MoveRotation(rb.rotation * addRotation);
    }
}
