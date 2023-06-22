using UnityEngine;
using System.Threading.Tasks;

public class Dice : MonoBehaviour
{
    public GameObject Bowl;
    private Rigidbody Rigidbody;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        UpdateDiceInfo();
    }

    private async void UpdateDiceInfo()
    {
        while (true)
        {
            if (this == null) return;

            if (IsGetDistance()) {
                gameObject.layer = 10;
                GameProcess.instance.RemoveTarget(gameObject);
                break;
            }

            if (Rigidbody.IsSleeping()) GameProcess.instance.AddSleepTarget(gameObject);
            await Task.Delay(100);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != Bowl) return;

        if (GameStart.instance.GameType == 0) {
            GameProcess.instance.InitSetDice();
            CameraDice.instance.InitCameraDice();

        } else if (GameProcess.instance.IsDroping == true)
        {
            Sound.instance.SoundHitToBowl();
            GameProcess.instance.IsDroping = false;
            GameStart.instance.CameraDice.enabled = false;
            GameStart.instance.CameraBowl.enabled = true;
            GameStart.instance.WaitType = 3;
            GameStart.instance.IsClick = false;
            if (GameStart.instance.RemainNudge != 0)
            {
                GameStart.instance.RemainNudgeText.color = new Color(1, 1, 1);
            }
            CameraBowl.instance.DiceOnBowl();
            CameraBowl.instance.InitCameraBowl();
            GameProcess.instance.InitSetCameraObject(GameStart.instance.BowlObject);
        }
    }

    private bool IsGetDistance()
    {
        Vector2 BowlPos = new Vector2(Bowl.transform.position.x, Bowl.transform.position.z);
        Vector2 DicePos = new Vector2(transform.position.x, transform.position.z);
        return 9.25 <= Vector2.Distance(BowlPos, DicePos);
    }
}
