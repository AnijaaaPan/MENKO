using UnityEngine;
using System.Threading.Tasks;

public class ScaneDice : MonoBehaviour
{
    public static ScaneDice instance;
    public bool IsScaning = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    async void Start()
    {
        while (true)
        {
            if (this == null) return;
            if (GameStart.instance.WaitType == 4 && IsScaning == false)
            {
                IsScaning = true;
                StartScaneDice();
            }
            await Task.Delay(10);
        }
    }

    private async void StartScaneDice()
    {
        RectTransform RectTransform = GetComponent<RectTransform>();
        for (int i = 0; i <= 100; i++)
        {
            if (RectTransform != null) RectTransform.localPosition = new Vector3(0, 0, i);
            await Task.Delay(15);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject ParentObject = collision.gameObject.transform.parent.gameObject;

        if (GameStart.instance.WaitType != 4 || IsScaning == false || !collision.gameObject.name.Contains("Dice Word")) return;
        if (GameScore.instance.DiceValues.Find(d => d.DiceObject == ParentObject) != null) return;

        GameScore.instance.AddDiceWord(ParentObject, collision.gameObject.name.Replace("Dice Word ", ""));
    }
}
