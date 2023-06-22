using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class CountFlash : MonoBehaviour
{
    private bool IsUpDown = false;
    private float DiffValue;
    private TextMeshProUGUI CountText;

    async void Start()
    {
        CountText = GetComponent<TextMeshProUGUI>();
        CountText.alpha = 1;

        while (true)
        {
            if (CountText == null) return;

            await Task.Delay(20);
            if (GameStart.instance.WaitType > 1)
            {
                CountText.alpha = 1;
                continue;
            }

            CountText.alpha = GetAlphaColor(CountText.alpha);
        }
    }

    private float GetAlphaColor(float alpha)
    {
        if (CountText.alpha >= 1 && IsUpDown == true)
        {
            IsUpDown = false;
        } else if (CountText.alpha <= 0 && IsUpDown == false)
        {
            IsUpDown = true;
        }

        DiffValue = IsUpDown == false ? -1 : 1;
        alpha += DiffValue * 0.05f;
        return alpha;
    }
}
