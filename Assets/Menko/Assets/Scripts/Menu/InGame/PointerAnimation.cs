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
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, nowPosZ + Mathf.PingPong(Time.time / 5, 0.5f));

        var addRotation = Quaternion.Euler(0, 7.5f, 0f);
        rb.MoveRotation(rb.rotation * addRotation);
    }
}
