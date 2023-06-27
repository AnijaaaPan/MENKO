using UnityEngine;

public class UpdateMenkoLayer : MonoBehaviour
{
    public Transform InitFieldMenkoTransform;

    private const float Radius = 10.5f; //移動出来る範囲

    private void Update()
    {
        if (gameObject.layer == 14) return;
        if (IsGetDistance()) return;

        UpdateMenko();
    }

    private void UpdateMenko()
    {
        gameObject.layer = 14;

        Outline MenkoOutline = GetComponent<Outline>();
        if (MenkoOutline) Destroy(MenkoOutline);
    }

    private bool IsGetDistance()
    {
        Vector2 InitFieldMenkoPos = new Vector2(InitFieldMenkoTransform.position.x, InitFieldMenkoTransform.position.z);
        Vector2 MenkoPos = new Vector2(transform.position.x, transform.position.z);
        return Radius > Vector2.Distance(MenkoPos, InitFieldMenkoPos);
    }
}
