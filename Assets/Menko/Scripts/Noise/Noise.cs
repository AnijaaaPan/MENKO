using UnityEngine;
using System.Threading.Tasks;

public class Noise : MonoBehaviour
{
    private float MoveY;
    private int IntervalTime;

    private RectTransform RectTransform;

    void Start()
    {
        RectTransform = GetComponent<RectTransform>();

        InitMove();
        MoveLineY();
    }

    private void InitMove()
    {
        MoveY = Random.Range(-5f, 5f);
        IntervalTime = Random.Range(10, 100);
    }

    private async void MoveLineY()
    {
        while (true)
        {
            if (RectTransform == null) return;

            float PosY = RectTransform.localPosition.y;
            if (PosY <= -540f || 540f <= PosY)
            {
                InitMove();
                RectTransform.localPosition = new Vector3(0, GetRandomYPos(), 0);
            };

            PosY += MoveY;
            RectTransform.localPosition = new Vector3(0, PosY, 0);
            await Task.Delay(IntervalTime);
        }
    }

    private float GetRandomYPos()
    {
        return Random.Range(-540f, 540f);
    }
}
