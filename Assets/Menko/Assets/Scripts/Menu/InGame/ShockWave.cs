using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public Cinemachine.CinemachineImpulseSource CinemachineImpulseSource;
    public bool isTrigger = false;

    private float powerMeterValue;

    void OnCollisionEnter(Collision collision)
    {
        if (isTrigger == true) return;
        isTrigger = true;

        powerMeterValue = GameProcess.instance.PowerMeterValue;
        if (powerMeterValue >= 0.8f)
        {
            ShowParticle.instance.Show();
        }

        UpdateColliders();
    }

    private void UpdateColliders()
    {
        float blastRadius = powerMeterValue * 5.0f;
        float blastPower = powerMeterValue * 12.5f;
        Vector3 blastPos = transform.position;

        Collider[] colliders = Physics.OverlapSphere(blastPos, blastRadius);
        foreach (Collider hit in colliders)
        {
            AddExplosionForce(hit, blastRadius, blastPower, blastPos);
        }
    }

    private void AddExplosionForce(Collider hit, float blastRadius, float blastPower, Vector3 blastPos)
    {
        if (hit.gameObject.name != "Side") return;

        GameObject MenkoObject = hit.gameObject.transform.parent.gameObject;
        Rigidbody rb = MenkoObject.GetComponent<Rigidbody>();
        if (rb == null || MenkoObject == gameObject) return;
        rb.AddExplosionForce(blastPower, blastPos, blastRadius, 3.0f, ForceMode.Impulse);

        CinemachineImpulseSource.GenerateImpulse(powerMeterValue * 3.5f);
    }
}
