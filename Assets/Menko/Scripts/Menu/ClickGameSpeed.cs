using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickGameSpeed : MonoBehaviour, IPointerClickHandler
{
    public int Type;
    public TextMeshProUGUI[] TextMeshProUGUIs;

    private void Start()
    {
        PlayerData PlayerData = Json.instance.Load();
        if (Type == PlayerData.Speed)
        {
            EnableDisableText(Type);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerData PlayerData = Json.instance.Load();
            if (PlayerData.Speed == Type) return;
            MoveNoises.instance.ClickPopNoise();

            EnableDisableText(PlayerData.Speed, true);
            PlayerData.Speed = Type;
            UpdateTimeScale();
            EnableDisableText(Type);

            Json.instance.Save(PlayerData);
        }
    }

    private void EnableDisableText(int index, bool disable = false)
    {
        float ColorValue = disable == false ? 0.75f : 0.25f;

        TextMeshProUGUI Text = TextMeshProUGUIs[index - 1];
        Text.color = new Color(ColorValue, ColorValue, ColorValue);
    }

    private void UpdateTimeScale()
    {
        if (Type == 1)
        {
            Time.timeScale = 0.5f;
        }
        else if (Type == 2)
        {
            Time.timeScale = 0.75f;
        }
        else if (Type == 3)
        {
            Time.timeScale = 1f;
        }
        else if (Type == 4)
        {
            Time.timeScale = 1.25f;
        }
        else
        {
            Time.timeScale = 1.5f;
        }
    }
}
