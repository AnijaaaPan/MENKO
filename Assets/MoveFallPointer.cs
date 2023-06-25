using UnityEngine;
using Menko.GameProcess;

public class MoveFallPointer : MonoBehaviour
{
    public GameObject JoyStickObject;
    public GameObject MoveFallPointerObject;

    public FloatingJoystick inputMove; //�����JoyStick

    private const float MoveSpeed = 2.5f; //�ړ����鑬�x

    void Update()
    {
        if (GameProcess.instance.ProcessState != ProcessState.WaitStart) return;
        if (GameProcess.instance.BattleTurn != BattleUserType.Player) return;

        bool isJoyStickActive = JoyStickObject.activeSelf;
        MoveFallPointerObject.SetActive(isJoyStickActive);
        if (!isJoyStickActive) return;

        //���X�e�B�b�N�ł̏c�ړ�
        MoveFallPointerObject.transform.position += MoveFallPointerObject.transform.forward * inputMove.Vertical * MoveSpeed * Time.deltaTime;
        //���X�e�B�b�N�ł̉��ړ�
        MoveFallPointerObject.transform.position += MoveFallPointerObject.transform.right * inputMove.Horizontal * MoveSpeed * Time.deltaTime;
    }
}