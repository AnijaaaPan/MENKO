using System.Threading.Tasks;
using UnityEngine;

public class CameraRing : MonoBehaviour
{
    public static CameraRing instance;

    public CameraMultiTarget cameraMultiTarget;

    private int MovePitch = 0; // 0: �����Ȃ�, 1: 90�x�ɋ߂Â�, 2: 0�x�ɋ߂Â�
    private int MoveYaw = 0; // 0: �����Ȃ�, 1: ���v���, 2: �����v���
    private int MovePadding = 0; // 0: �����Ȃ�, 1: �g��, 2: �k��

    private float PitchMin = 35f;
    private float PitchMax = 75f;
    private float YawMin = 0f;
    private float YawMax = 360f;
    private float PaddingMin = 6.5f;
    private float PaddingMax = 12.5f;
    private float DiffPaddingMax = 0.05f;

    private float InitPitch = 0;
    private float InitYaw = 0;
    private float InitPadding = 0;

    private float DiffPitch = 0;
    private float DiffYaw = 0;
    private float DiffPadding = 0;

    private readonly int IntervalTime = 50;

    private int InitCount = 0;
    private int RandomCount = 0;

    private bool IsStop = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitCameraRing();
        UpdateCamera();
    }

    private async void UpdateCamera()
    {
        if (IsStop) return;
        if (cameraMultiTarget == null) return;

        if (InitCount % RandomCount == 0)
        {
            InitCameraRing();
        }

        UpdatePitch();
        UpdateYaw();
        UpdatePadding();

        InitCount++;
        await Task.Delay(IntervalTime);

        UpdateCamera();
    }

    public void InitCameraRing()
    {
        MovePitch = RandomChoiceType();
        MoveYaw = RandomChoiceType();
        MovePadding = RandomChoiceType();

        InitPitch = Random.Range(PitchMin, PitchMax);
        InitYaw = Random.Range(YawMin, YawMax);
        InitPadding = Random.Range(PaddingMin, PaddingMax);

        cameraMultiTarget.Pitch = InitPitch;
        cameraMultiTarget.Roll = 0;
        cameraMultiTarget.Yaw = InitYaw;
        cameraMultiTarget.PaddingDown = InitPadding;
        cameraMultiTarget.PaddingLeft = InitPadding;
        cameraMultiTarget.PaddingRight = InitPadding;
        cameraMultiTarget.PaddingUp = InitPadding;

        DiffPitch = Random.Range(0f, 0.25f);
        DiffYaw = Random.Range(0f, 0.75f);
        DiffPadding = Random.Range(0f, DiffPaddingMax);

        InitCount = 0;
        RandomCount = Random.Range(250, 500);
    }

    private int RandomChoiceType()
    {
        int RandomRange = Random.Range(1, 101);
        if (RandomRange <= 20) return 0;
        if (RandomRange <= 60) return 1;
        return 2;
    }

    private void UpdatePitch()
    {
        if (MovePitch == 0) return;

        float UpdateValue = MovePitch == 1 ? DiffPitch : -DiffPitch;
        cameraMultiTarget.Pitch += UpdateValue;

        if (cameraMultiTarget.Pitch >= PitchMax) MovePitch = 2;
        else if (cameraMultiTarget.Pitch <= PitchMin) MovePitch = 1;
    }

    private void UpdateYaw()
    {
        if (MoveYaw == 0) return;

        float UpdateValue = MoveYaw == 1 ? DiffYaw : -DiffYaw;
        cameraMultiTarget.Yaw += UpdateValue;
    }

    private void UpdatePadding()
    {
        if (MovePadding == 0) return;

        float UpdateValue = MovePadding == 1 ? DiffPadding : -DiffPadding;
        cameraMultiTarget.PaddingDown += UpdateValue;
        cameraMultiTarget.PaddingLeft += UpdateValue;
        cameraMultiTarget.PaddingRight += UpdateValue;
        cameraMultiTarget.PaddingUp += UpdateValue;

        if (cameraMultiTarget.PaddingDown >= PaddingMax) MovePadding = 2;
        else if (cameraMultiTarget.PaddingDown <= PaddingMin) MovePadding = 1;
    }

    public void Stop()
    {
        IsStop = true;
    }

    public void ReStart()
    {
        IsStop = false;
        UpdateCamera();
    }

    public void WaitStart()
    {
        PaddingMax = 12.5f;
        PaddingMin = 10;
        PitchMax = 45;
        PitchMin = 35;
        InitCount = 1;
        RandomCount = 1;
    }

    public void ResetCameraMove()
    {
        PitchMin = 35f;
        PitchMax = 75f;
        YawMin = 0f;
        YawMax = 360f;
        PaddingMin = 6.5f;
        PaddingMax = 12.5f;
        DiffPaddingMax = 0.05f;
        InitCount = 1;
        RandomCount = 1;
    }
}
