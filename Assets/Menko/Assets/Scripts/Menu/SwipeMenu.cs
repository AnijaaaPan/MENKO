using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    [SerializeField]
    Scrollbar ScrollBar;

    private int Lenght;
    private float Distance;
    private int SelectMenkoId = -1;
    private float ScrollPos = 0;
    private float[] Positions;

    private void Update()
    {
        Lenght = transform.childCount;
        Positions = new float[Lenght];
        Distance = 1f / (Lenght - 1f);
        for (int i = 0; i < Lenght; i++)
        {
            Positions[i] = Distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            ScrollPos = ScrollBar.value;
        }
        else
        {
            for (int i = 0; i < Lenght; i++)
            {
                if (ScrollPos < Positions[i] + (Distance / 2) && ScrollPos > Positions[i] - (Distance / 2))
                {
                    UpdateSelectMenkoId(i);
                }
            }
        }

        UpdateSelectMenu();
    }

    private void UpdateSelectMenkoId(int index)
    {
        SelectMenkoId = index;
        ScrollBar.value = Mathf.Lerp(ScrollBar.value, Positions[index], 0.1f);
    }

    private void UpdateSelectMenu()
    {
        for (int i = 0; i < Lenght; i++)
        {
            if (ScrollPos < Positions[i] + (Distance / 2) && ScrollPos > Positions[i] - (Distance / 2))
            {
                for (int a = 0; a < Lenght; a++)
                {
                    Transform chileTransform = transform.GetChild(a);

                    float value = IsSelected(a) ? 1f : 0.7f;

                    Vector3 newVector3 = new(value, value, value);
                    chileTransform.localScale = Vector3.Lerp(chileTransform.localScale, newVector3, 0.1f);

                    Quaternion newQuaternion = GenerateQuaternion(a);
                    chileTransform.localRotation = Quaternion.Lerp(chileTransform.localRotation, newQuaternion, 0.1f);
                }
            }
        }
    }

    private Quaternion GenerateQuaternion(int id)
    {
        if (id < SelectMenkoId) return Quaternion.Euler(0f, -30f, 0f);
        if (id > SelectMenkoId) return Quaternion.Euler(0f, 30f, 0f);
        return Quaternion.Euler(0f, 0, 0f);
    }

    private bool IsSelected(int index)
    {
        return index == SelectMenkoId;
    }
}
