using UnityEngine;

public class ShowParticle : MonoBehaviour
{
    public static ShowParticle instance;

    [SerializeField]
    GameObject ShowParticleObject;

    private GameObject[] ParticleObjects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ParticleObjects = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            ParticleObjects[i] = transform.GetChild(i).gameObject;
        }
    }

    public void Show()
    {
        Vector3 FallPointPos = GameProcess.instance.FallPointPos;
        Vector3 InitPos = new(FallPointPos.x, FallPointPos.y, 1.25f);
        Quaternion InitQ = Quaternion.Euler(0, 0, 0);

        GameObject particleObject = GetParticleObject();
        GameObject spawnedHit = Instantiate(particleObject, InitPos, InitQ);
        spawnedHit.SetActive(true);

        spawnedHit.transform.SetParent(ShowParticleObject.transform, false);
    }

    private GameObject GetParticleObject()
    {
        System.Random random = new();
        int randomIndex = random.Next(ParticleObjects.Length);
        return ParticleObjects[randomIndex];
    }
}
