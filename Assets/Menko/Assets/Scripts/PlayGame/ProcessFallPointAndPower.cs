using UnityEngine;

public class ProcessFallPointAndPower : MonoBehaviour
{
    public GameObject JoyStickObject;
    public GameObject MoveFallPointerObject;

    public FloatingJoystick InputMove; //�����JoyStick

    private const float MoveSpeed = 2.5f; //�ړ����鑬�x

    private void Update()
    {
        bool isJoyStickActive = JoyStickObject.activeInHierarchy;
        if (!isJoyStickActive)
        {
            MoveFallPointerObject.SetActive(false);
            GameProcess.instance.UpdateProcessMenkoFalling();
        };

        UpdateFallPointerPos();
    }

    public void Run()
    {
        MoveFallPointerObject.SetActive(true);
        CameraRing.instance.Stop();
    }

    private void UpdateFallPointerPos()
    {
        //���X�e�B�b�N�ł̏c�ړ�
        MoveFallPointerObject.transform.position += MoveFallPointerObject.transform.forward * InputMove.Vertical * MoveSpeed * Time.deltaTime;
        //���X�e�B�b�N�ł̉��ړ�
        MoveFallPointerObject.transform.position += MoveFallPointerObject.transform.right * InputMove.Horizontal * MoveSpeed * Time.deltaTime;
    }
}
