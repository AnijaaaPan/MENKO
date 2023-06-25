using UnityEngine;
using Menko.GameProcess;

public class MoveFallPointer : MonoBehaviour
{
    public GameObject JoyStickObject;
    public GameObject MoveFallPointerObject;

    public FloatingJoystick inputMove; //左画面JoyStick

    private const float MoveSpeed = 2.5f; //移動する速度

    void Update()
    {
        if (GameProcess.instance.ProcessState != ProcessState.WaitStart) return;
        if (GameProcess.instance.BattleTurn != BattleUserType.Player) return;

        bool isJoyStickActive = JoyStickObject.activeSelf;
        MoveFallPointerObject.SetActive(isJoyStickActive);
        if (!isJoyStickActive) return;

        //左スティックでの縦移動
        MoveFallPointerObject.transform.position += MoveFallPointerObject.transform.forward * inputMove.Vertical * MoveSpeed * Time.deltaTime;
        //左スティックでの横移動
        MoveFallPointerObject.transform.position += MoveFallPointerObject.transform.right * inputMove.Horizontal * MoveSpeed * Time.deltaTime;
    }
}