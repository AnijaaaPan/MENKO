using UnityEngine;
using System.Threading.Tasks;

public class MoveNoises : MonoBehaviour
{
    public static MoveNoises instance;

    public GameObject InitNoise;
    public int NoiseCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < NoiseCount; i++)
        {
            GameObject NoiseObject = Instantiate(InitNoise);

            float RandomYPos = GetRandomYPos();

            NoiseObject.SetActive(true);
            NoiseObject.transform.SetParent(transform);

            RectTransform RectTransform = NoiseObject.GetComponent<RectTransform>();
            RectTransform.localPosition = new Vector3(0, RandomYPos, 0);
            RectTransform.localScale = Vector3.one;
        }
    }

    public void ClickPopNoise()
    {
        for (int i = 1; i <= NoiseCount; i++)
        {
            GameObject NoiseObject = transform.GetChild(i).gameObject;
            PopNoise(NoiseObject);
        }
    }

    public void VisibleAllNoise(bool Visible = true)
    {
        for (int i = 1; i <= NoiseCount; i++)
        {
            GameObject NoiseObject = transform.GetChild(i).gameObject;
            if (NoiseObject == null) return;

            LineRenderer LineRenderer = NoiseObject.GetComponent<LineRenderer>();
            float ColorAlpha = Visible == true ? Random.Range(0.1f, 0.2f) : 0;
            LineRenderer.colorGradient = CreateColor(ColorAlpha);
        }
    }

    private float GetRandomYPos()
    {
        return Random.Range(-540f, 540f);
    }

    private async void PopNoise(GameObject NoiseObject)
    {
        LineRenderer LineRenderer = NoiseObject.GetComponent<LineRenderer>();
        int RandomIntervalTime = Random.Range(10, 25);
        float RandomColorAlpha = Random.Range(0.1f, 0.2f);

        float[] ColorAlphaList = new float[11] {
            0, RandomColorAlpha * 1, RandomColorAlpha * 2, RandomColorAlpha * 3, RandomColorAlpha * 4, RandomColorAlpha * 5,
               RandomColorAlpha * 4, RandomColorAlpha * 3, RandomColorAlpha * 2, RandomColorAlpha * 1, 0
        };

        for (int i = 0; i < ColorAlphaList.Length; i++)
        {
            float ColorAlpha = ColorAlphaList[i];
            LineRenderer.colorGradient = CreateColor(ColorAlpha);
            await Task.Delay(RandomIntervalTime);
        }
    }

    private Gradient CreateColor(float alpha)
    {
        GradientColorKey[] colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.white;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.white;
        colorKey[1].time = 1.0f;

        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = alpha;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = alpha;
        alphaKey[1].time = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys(colorKey, alphaKey);
        return gradient;
    }
}
