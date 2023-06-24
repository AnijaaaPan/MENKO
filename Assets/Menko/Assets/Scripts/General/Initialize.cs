using UnityEngine;

public class Initialize : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void RuntimeInitializeApplication()
    {
        Application.targetFrameRate = 60;
    }
}