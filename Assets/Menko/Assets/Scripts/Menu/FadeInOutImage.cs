using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutImage : MonoBehaviour
{
    public static FadeInOutImage instance;
    public Image Image;

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
        await FadeInOut(true, 0.01f, 75);
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
}
