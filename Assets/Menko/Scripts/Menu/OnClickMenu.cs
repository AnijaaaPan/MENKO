using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

[System.Serializable]
public class ObjectPair
{
	public Image Image;
	public TextMeshProUGUI TextMeshProUGUI;
}

public class OnClickMenu : MonoBehaviour, IPointerClickHandler
{
	public GameObject Option;
	public GameObject Arcade;
	public GameObject FreeRole;
	public GameObject AutoPlay;
	public GameObject Quit;

	public GameObject TitleObject;
	public RectTransform TitleRectTransform;
	public GameObject OptionObject;

	public ObjectPair ObjectPair;
	public CanvasGroup StartMenuCanvas;

	private float time;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			ObjectFlashing(eventData.pointerEnter);
		}
	}

	private async void ObjectFlashing(GameObject Object)
	{
		for (int i = 0; i < 20; i++)
		{
			ObjectPair.TextMeshProUGUI.color = GetAlphaColor(ObjectPair.TextMeshProUGUI.color);
			ObjectPair.Image.color = ObjectPair.TextMeshProUGUI.color;
			await Task.Delay(6);
		}

		ObjectPair.TextMeshProUGUI.color = new Color(0, 0, 0, 0);
		ObjectPair.Image.color = ObjectPair.TextMeshProUGUI.color;

		ZoomIn(-10, 0.9f);
		await Task.Delay(10);
		ZoomIn(-20, 0.8f);
		await Task.Delay(10);
		ZoomIn(-30, 0.7f);
		await Task.Delay(10);
		ZoomIn(-40, 0.6f);
		await Task.Delay(10);
		ZoomIn(-60, 0.4f);
		await Task.Delay(10);
		ZoomIn(-80, 0.2f);
		await Task.Delay(10);
		ZoomIn(-100, 0.1f);
		await Task.Delay(10);
		ZoomIn(-150, 0f);

		TitleObject.SetActive(false);

		if (Object == Quit)
		{
			QuitGame();
			return;
		}

		ObjectFunc(Object);
	}

	private Color GetAlphaColor(Color color)
	{
		time += Time.unscaledDeltaTime * 20;
		color.a = Mathf.Sin(time) * 0.5f + 0.5f;
		return color;
	}

	private void ZoomIn(int positionZ, float colorAlpha)
	{
		TitleRectTransform.localPosition = new Vector3(0, 0, positionZ);
		StartMenuCanvas.alpha = colorAlpha;
	}

	private void ObjectFunc(GameObject Object)
    {
		if (Object == Option)
		{
			OptionObject.SetActive(true);
			return;
		}

		TitleObject.SetActive(false);
		if (Object == Arcade)
		{
			GameStart.instance.InitGame(1);
		}
		else if (Object == FreeRole)
		{
			GameStart.instance.InitGame(2);
		} else
		{
			GameStart.instance.InitGame(3);
		}
	}

	private void QuitGame()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_STANDALONE
			UnityEngine.Application.Quit();
		#endif
	}
}
