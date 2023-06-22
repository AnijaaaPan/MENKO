using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

[System.Serializable]
public class PosDice
{
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    public float MinZ;
    public float MaxZ;
}

public class GameProcess : MonoBehaviour
{
    public static GameProcess instance;

    public CameraMultiTarget cameraMultiTarget;
    public GameObject InitDice;
    public MeshCollider DiceMeshCollider;
    public bool IsDroping = true;

    public List<GameObject> IsSleepingDices = new List<GameObject>();
    public int AllDiceSleep = 0;

    private List<PosDice> PosDices = new List<PosDice>();
    private List<GameObject> OnBowlDices = new List<GameObject>();
    private List<GameObject> Dices = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        InitSetDice();
        RotateYDice();
    }

    private async void RotateYDice()
    {
        while (true)
        {
            if (this == null) return;
            if (IsDroping == false) break;

            transform.Rotate(new Vector3(0, 30 * Time.deltaTime, 0));
            await Task.Delay(100);
        }
    }

    public void InitSetCameraObject(GameObject[] Objects)
    {
        cameraMultiTarget.SetTargets(Objects);
    }

    public void InitSetDice()
    {
        DeleteDices();
        SetDices();
    }

    public void DeleteDices()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        PosDices = new List<PosDice>();
        Dices = new List<GameObject>();
        OnBowlDices = new List<GameObject>();
        InitSetCameraObject(Dices.ToArray());
    }

    public void SetDices()
    {
        for (int i = 0; i < GameStart.instance.DiceCount; i++)
        {
            Vector3 InitPos = InitRandomPos();
            Quaternion InitQ = Quaternion.Euler(0, 0, Random.Range(-180f, 180f));

            GameObject DiceObject = Instantiate(InitDice, InitPos, InitQ);
            DiceObject.name = $"Dice{i}";
            Rigidbody Rigidbody = DiceObject.GetComponent<Rigidbody>();
            if (GameStart.instance.WaitType != 0) Rigidbody.drag = 0;
            Rigidbody.angularVelocity = new Vector3(getRandomPower(), getRandomPower(), getRandomPower());
            Rigidbody.AddForce(new Vector3(0, -15, 0), ForceMode.Impulse);

            DiceObject.SetActive(true);
            DiceObject.transform.SetParent(transform);
            Dices.Add(DiceObject);
        }

        OnBowlDices = Dices;
        InitSetCameraObject(Dices.ToArray());
    }

    private float getRandomPower()
    {
        float power = GameStart.instance.WaitType == 0 ? 0.5f : 2f;
        return Random.Range(-power, power);
    }

    Vector3 InitRandomPos()
    {
        float DiceSize = 2.5f;

        while (true)
        {
            Vector3 SpawnPos = (2.5f + GameStart.instance.DiceCount * 0.125f) * Random.insideUnitSphere + InitDice.transform.position;

            float RandomX = Random.Range(SpawnPos.x, SpawnPos.x);
            float RandomY = Random.Range(SpawnPos.y, SpawnPos.y);
            float RandomZ = Random.Range(SpawnPos.z, SpawnPos.z);
            if (CheckDistinctDice(RandomX, RandomY, RandomZ)) continue;

            PosDice PosDice = new PosDice()
            {
                MinX = RandomX - DiceSize,
                MaxX = RandomX + DiceSize,
                MinY = RandomY - DiceSize,
                MaxY = RandomY + DiceSize,
                MinZ = RandomZ - DiceSize,
                MaxZ = RandomZ + DiceSize
            };
            PosDices.Add(PosDice);
            return new Vector3(RandomX, RandomY, RandomZ);
        }
    }

    private bool CheckDistinctDice(float RandomX, float RandomY, float RandomZ)
    {
        for (int i = 0; i < PosDices.Count; i++)
        {
            PosDice PosDice = PosDices[i];
            if (PosDice.MinX <= RandomX && RandomX <= PosDice.MaxX &&
                PosDice.MinY <= RandomY && RandomY <= PosDice.MaxY &&
                PosDice.MinZ <= RandomZ && RandomZ <= PosDice.MaxZ)
                return true;
        }
        return false;
    }

    public void RemoveTarget(GameObject TargetObject)
    {
        OnBowlDices.Remove(TargetObject);
    }

    public async void AddSleepTarget(GameObject TargetObject)
    {
        if (IsSleepingDices.Contains(TargetObject)) return;
        IsSleepingDices.Add(TargetObject);

        if (IsSleepingDices.Count == OnBowlDices.Count)
        {
            AllDiceSleep = 1;
            await Task.Delay(1500);
            if (AllDiceSleep == 1)
            {
                GameStart.instance.WaitType = 4;
            }
        }
    }

    public void InitCamera()
    {
        cameraMultiTarget.Pitch = 0;
        cameraMultiTarget.Roll = 0;
        cameraMultiTarget.Yaw = 0;
        cameraMultiTarget.PaddingDown = 5;
        cameraMultiTarget.PaddingLeft = 5;
        cameraMultiTarget.PaddingRight = 5;
        cameraMultiTarget.PaddingUp = 5;
    }
}
