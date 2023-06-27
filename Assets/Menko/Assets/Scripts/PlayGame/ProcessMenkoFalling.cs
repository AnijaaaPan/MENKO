using Menko.GameProcess;
using Menko.MenkoData;
using Menko.ScriptableObjects;
using System.Threading.Tasks;
using UnityEngine;

public class ProcessMenkoFalling : MonoBehaviour
{
    [SerializeField]
    GameObject InGameMenkoObjects;

    [SerializeField]
    Cinemachine.CinemachineImpulseSource CinemachineImpulseSource;

    [SerializeField]
    Transform InitFieldMenkoTransform;

    private GameObject FallMenkoObject = null;

    private void Update()
    {
        if (FallMenkoObject == null) return;

        ShockWave MenkoShockWave = FallMenkoObject.GetComponent<ShockWave>();
        if (!MenkoShockWave.isTrigger) return;

        GameProcess.instance.UpdateProcessMenkoFallEnd();
    }

    public void Run()
    {
        FallMenkoObject = SetFallMenko();

        GameObject[] newCameraObjects = new[] { FallMenkoObject };
        GameProcess.instance.InitSetCameraObject(newCameraObjects);
        GameProcess.instance.EnableCameraMenko();

        RandomTimeScale();
    }

    private GameObject SetFallMenko()
    {
        BattleUserState battleUserState = GameProcess.instance.GetBattleUserState();
        MenkoData data = battleUserState.SetMenkoData;

        Vector3 FallPointPos = GameProcess.instance.FallPointPos;
        Vector3 InitPos = new(FallPointPos.x, FallPointPos.z, -FallPointPos.y);
        Quaternion InitQ = Quaternion.Euler(0, 0, 0);

        GameObject MenkoPrefab = data.GetPrefab();
        GameObject MenkoObject = Instantiate(MenkoPrefab, InitPos, InitQ);
        MenkoObject.name = "UserMenko";
        MenkoMesh.Update(MenkoObject, data);

        Outline MenkoOutline = MenkoObject.AddComponent<Outline>();
        MenkoOutline.OutlineMode = Outline.Mode.OutlineVisible;
        MenkoOutline.OutlineColor = GetOutlineColor();
        MenkoOutline.OutlineWidth = 2.5f;

        ShockWave MenkoShockWave = MenkoObject.AddComponent<ShockWave>();
        MenkoShockWave.CinemachineImpulseSource = CinemachineImpulseSource;

        UpdateMenkoLayer UpdateMenkoLayer = MenkoObject.AddComponent<UpdateMenkoLayer>();
        UpdateMenkoLayer.InitFieldMenkoTransform = InitFieldMenkoTransform;

        MenkoObject.SetActive(true);
        MenkoObject.transform.SetParent(InGameMenkoObjects.transform, false);

        float PowerMeterValue = GameProcess.instance.PowerMeterValue;
        Rigidbody rigidbody = MenkoObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(0, -(PowerMeterValue * 2000), 0);

        return MenkoObject;
    }

    private Color GetOutlineColor()
    {
        if (GameProcess.instance.IsPlayerTurn())
        {
            return new Color(0.25f, 0, 1, 0.5f);
        }
        return new Color(1, 0.25f, 0, 0.5f);
    }

    private async void RandomTimeScale()
    {
        await Task.Delay(Random.Range(75, 125));

        float timeScale = Random.Range(0.075f, 0.125f);
        Time.timeScale *= timeScale;

        Sound.instance.SoundFaa();
        await Task.Delay(Random.Range(1250, 1750));
        Sound.instance.SoundEffect.Stop();

        Time.timeScale = 1;
    }
}
