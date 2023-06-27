using UnityEngine;
using UnityEngine.UI;

public class ProcessFallPointAndPower : MonoBehaviour
{
    public GameObject JoyStickObject;
    public GameObject FallPointerCanvasObject;
    public GameObject FallPointerObject;

    public GameObject PointImageObject;
    public GameObject PowerMeterObject;
    public Image powerMeterImage;

    public FloatingJoystick InputMove; //�����JoyStick

    private Rigidbody FallPointerCanvasRigidbody;
    private Rigidbody FallPointerRigidbody;
    private Vector3 InitMoveFallPointerPos;

    private float PowerMeterSpeedRate = 0.75f;
    private float PowerMeterElapsedTime = 0;
    private const float MoveSpeed = 125f; //�ړ����鑬�x
    private const float Radius = 10f; //�ړ��o����͈�

    private void Start()
    {
        FallPointerCanvasRigidbody = FallPointerCanvasObject.GetComponent<Rigidbody>();
        FallPointerRigidbody = FallPointerObject.GetComponent<Rigidbody>();
        InitMoveFallPointerPos = FallPointerObject.transform.localPosition;
    }

    private void Update()
    {
        bool isJoyStickActive = JoyStickObject.activeInHierarchy;
        if (!isJoyStickActive)
        {
            GameProcess.instance.FallPointPos = FallPointerObject.transform.localPosition;
            GameProcess.instance.FallPointPos.z = 40;
            GameProcess.instance.PowerMeterValue = powerMeterImage.fillAmount;

            FallPointerObject.SetActive(false);
            FallPointerCanvasObject.SetActive(false);
            GameProcess.instance.UpdateProcessMenkoFalling();
        };

        UpdateFallPointerPos();
        Restriction();
        UpdatePowerMeter();
    }

    public void Run()
    {
        FallPointerObject.SetActive(true);
        FallPointerCanvasObject.SetActive(true);
    }

    private void UpdateFallPointerPos()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        float inputVertical = InputMove.Vertical * Time.deltaTime;
        float inputHorizontal = InputMove.Horizontal * Time.deltaTime;

        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
        FallPointerRigidbody.velocity = moveForward * MoveSpeed + new Vector3(0, FallPointerRigidbody.velocity.y, 0);
        FallPointerCanvasRigidbody.velocity = FallPointerRigidbody.velocity;
    }

    private void Restriction()
    {
        if (Vector3.Distance(FallPointerObject.transform.localPosition, InitMoveFallPointerPos) <= Radius) return;

        Vector3 nor = FallPointerObject.transform.localPosition - InitMoveFallPointerPos;
        nor.Normalize();
        FallPointerObject.transform.localPosition = nor * Radius;
        FallPointerCanvasObject.transform.localPosition = FallPointerObject.transform.localPosition;
    }

    private void UpdatePowerMeter()
    {
        PowerMeterElapsedTime += Time.deltaTime * PowerMeterSpeedRate;
        powerMeterImage.fillAmount = Mathf.PingPong(PowerMeterElapsedTime, 1.035f);
    }
}
