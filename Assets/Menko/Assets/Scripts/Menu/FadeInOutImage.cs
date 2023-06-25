using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutImage : MonoBehaviour
{
    public static FadeInOutImage instance;
    public Image Image;

    public bool isTitle = true;
    private Color Color = Color.black;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    async void Start()
    {
        await FadeInOut(true, 0.01f, 100);
        await IsTitle();
    }

    public async Task FadeInOut(bool IsIn, float ChangeValue, int Delay)
    {
        for (int i = 0; i < 40; i++)
        {
            Color.a += IsIn == true ? -ChangeValue : ChangeValue;
            if (this != null) Image.color = Color;

            await Task.Delay(Delay);
        }
    }

    private async Task IsTitle()
    {
        if (!isTitle) return;
        await FadeInOut(false, 0.01f, 300);

        if (!isTitle) return;
        await FadeInOut(true, 0.01f, 300);

        await IsTitle();
    }
}
