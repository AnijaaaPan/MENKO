using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickQuality : MonoBehaviour, IPointerClickHandler
{
	public int Type;
    public TextMeshProUGUI[] TextMeshProUGUIs;

    private void Start()
    {
        PlayerData PlayerData = Json.instance.Load();
        if (Type == PlayerData.Quality)
        {
            EnableDisableText(Type);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerData PlayerData = Json.instance.Load();
            if (PlayerData.Quality == Type) return;
            MoveNoises.instance.ClickPopNoise();

            EnableDisableText(PlayerData.Quality, true);
            PlayerData.Quality = Type;
            UpdateResolution();
            EnableDisableText(Type);

            Json.instance.Save(PlayerData);
        }
    }

    private void EnableDisableText(int index, bool disable=false)
    {
        float ColorValue = disable == false ? 0.75f : 0.25f;

        TextMeshProUGUI Text = TextMeshProUGUIs[index - 1];
        Text.color = new Color(ColorValue, ColorValue, ColorValue);
    }

    private void UpdateResolution()
    {
        if (Type == 1) {
            Screen.SetResolution(1920, 1080, false);
        } else if (Type == 2) {
            Screen.SetResolution(2560, 1440, false);
        } else {
            Screen.SetResolution(3840, 2160, false);
        }
    }
}
