using UnityEngine;

public class ProcessFallPointAndPower : MonoBehaviour
{
    public GameObject JoyStickObject;
    public GameObject MoveFallPointerObject;

    public FloatingJoystick InputMove; //左画面JoyStick

    private const float MoveSpeed = 2.5f; //移動する速度

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
        //左スティックでの縦移動
        MoveFallPointerObject.transform.position += MoveFallPointerObject.transform.forward * InputMove.Vertical * MoveSpeed * Time.deltaTime;
        //左スティックでの横移動
        MoveFallPointerObject.transform.position += MoveFallPointerObject.transform.right * InputMove.Horizontal * MoveSpeed * Time.deltaTime;
    }
}
