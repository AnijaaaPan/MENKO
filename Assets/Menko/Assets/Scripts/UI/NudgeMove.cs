using UnityEngine;
using System.Threading.Tasks;


public class NudgeMove : MonoBehaviour
{
    public async void Start()
    {
        float size = 0;
        RectTransform RectTransform = GetComponent<RectTransform>();

        while (true)
        {
            if (RectTransform == null) return;

            if (size >= 35)
            {
                size = 0;
            }
            RectTransform.sizeDelta = new Vector2(size, size);

            size += 2.5f;
            await Task.Delay(20);
        }
    }
}
