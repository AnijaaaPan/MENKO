using UnityEngine;

public class ProcessFallPointAndPower : MonoBehaviour
{
    public GameObject JoyStickObject;
    public GameObject MoveFallPointerObject;

    public FloatingJoystick InputMove; //左画面JoyStick

    private Rigidbody rb;
    private Vector3 InitMoveFallPointerPos;

    private const float MoveSpeed = 75f; //移動する速度
    private const float Radius = 10f; //移動出来る範囲

    private void Start()
    {
        rb = MoveFallPointerObject.GetComponent<Rigidbody>();
        InitMoveFallPointerPos = MoveFallPointerObject.transform.localPosition;
    }

    private void Update()
    {
        bool isJoyStickActive = JoyStickObject.activeInHierarchy;
        if (!isJoyStickActive)
        {
            MoveFallPointerObject.SetActive(false);
            GameProcess.instance.UpdateProcessMenkoFalling();
        };

        UpdateFallPointerPos();
        Restriction();
    }

    public void Run()
    {
        MoveFallPointerObject.SetActive(true);
    }

    private void UpdateFallPointerPos()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        float inputVertical = InputMove.Vertical * Time.deltaTime;
        float inputHorizontal = InputMove.Horizontal * Time.deltaTime;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
        rb.velocity = moveForward * MoveSpeed + new Vector3(0, rb.velocity.y, 0);
    }

    private void Restriction()
    {
        if (Vector3.Distance(MoveFallPointerObject.transform.localPosition, InitMoveFallPointerPos) <= Radius) return;

        Vector3 nor = MoveFallPointerObject.transform.localPosition - InitMoveFallPointerPos;
        nor.Normalize();
        MoveFallPointerObject.transform.localPosition = nor * Radius;
    }
}
