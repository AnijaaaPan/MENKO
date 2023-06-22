using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnHoverEscapeImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Image CircleImage;
	public Image EscapeImage;

	public void OnPointerEnter(PointerEventData eventData)
	{
		int CircleColor = CircleImage == null ? 0 : 1;
		int EscapeColor = EscapeImage == null ? 0 : 1;

		if (CircleImage) CircleImage.color = new Color(0.75f, 0.75f, 0.75f, CircleColor);
		if (EscapeImage) EscapeImage.color = new Color(0.75f, 0.75f, 0.75f, EscapeColor);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		int CircleColor = CircleImage == null ? 0 : 1;
		int EscapeColor = EscapeImage == null ? 0 : 1;

		if (CircleImage) CircleImage.color = new Color(0.25f, 0.25f, 0.25f, CircleColor);
		if (EscapeImage) EscapeImage.color = new Color(0.25f, 0.25f, 0.25f, EscapeColor);
	}
}
