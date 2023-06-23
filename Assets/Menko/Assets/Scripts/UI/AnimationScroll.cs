using UnityEngine;
using System.Threading.Tasks;


public class AnimationScroll : MonoBehaviour
{
    public RectTransform ScrollImage;

    private int InitValue = 50;

    public async void Start()
    {
        CanvasGroup CanvasGroup = GetComponent<CanvasGroup>();

        while (true)
        {
            if (ScrollImage == null || this == null) return;

            await Task.Delay(2800);

            if (GameStart.instance.WaitType > 1)
            {
                if (CanvasGroup != null) CanvasGroup.alpha = 0;
                continue;
            }

            for (int i = 1; i <= 10; i++)
            {
                if (CanvasGroup != null) CanvasGroup.alpha = i * 0.1f;
                await Task.Delay(30);
            }

            while (true)
            {
                if (ScrollImage == null || this == null) break;
                if (ScrollImage.localPosition.y >= 175) break;

                InitValue += 7;
                if (ScrollImage != null) ScrollImage.localPosition = new Vector3(0, InitValue, 0);
                await Task.Delay(10);
            }

            for (int i = 10; i >= 0; i--)
            {
                if (CanvasGroup != null) CanvasGroup.alpha = i * 0.1f;
                await Task.Delay(50);
            }

            InitValue = 60;
            if (ScrollImage != null) ScrollImage.localPosition = new Vector3(0, InitValue, 0);
        }
    }
}
