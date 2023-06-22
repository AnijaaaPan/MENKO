using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class OnHoverMenu : MonoBehaviour, IPointerEnterHandler
{
	public GameObject Option;
	public GameObject Arcade;
	public GameObject FreeRole;
	public GameObject AutoPlay;
	public GameObject Quit;

	public bool isOption = false;

	private string Name;
	private int NameLength;

	void Start()
    {
		Name = gameObject.name;
		NameLength = Name.Length;
	}

    public void OnPointerEnter(PointerEventData eventData)
	{
		if (isAlreadyHover(eventData.pointerEnter) == true) return;

		InitMenu();
		VisibleMenu(eventData.pointerEnter);
	}

	private bool isAlreadyHover(GameObject Object)
	{
		if (isOption == true)
        {
			return Object.transform.Find("Text").gameObject.activeSelf;
		} else
        {
			return Object.transform.Find("Button").gameObject.activeSelf;
		}
	}

	private void InitMenu()
	{
		Option.transform.Find("Text").gameObject.SetActive(false);
		InitButtonMenu(Arcade);
		InitButtonMenu(FreeRole);
		InitButtonMenu(AutoPlay);
		InitButtonMenu(Quit);
	}

	private void InitButtonMenu(GameObject Object)
	{
		TextMeshProUGUI Text = Object.GetComponent<TextMeshProUGUI>();
		Text.enabled = true;
		Object.transform.Find("Button").gameObject.SetActive(false);
	}

	private void VisibleMenu(GameObject Object)
	{
		GameObject SelectObject;
		TextMeshProUGUI SelectText;

		if (isOption == true)
		{
			SelectObject = Option.transform.Find("Text").gameObject;
			SelectObject.SetActive(true);

			SelectText = SelectObject.GetComponent<TextMeshProUGUI>();
			SlotText(SelectText);
			return;
        }

		TextMeshProUGUI Text = Object.GetComponent<TextMeshProUGUI>();
		Text.enabled = false;

		SelectObject = Object.transform.Find("Button").gameObject;
		SelectObject.SetActive(true);
		GameObject TextObject = SelectObject.transform.Find("Text").gameObject;

		SelectText = TextObject.GetComponent<TextMeshProUGUI>();
		SlotText(SelectText);
	}

	private async void SlotText(TextMeshProUGUI Text)
	{		
		for (int i = 0; i < 5; i++)
		{
			string randomText = RandomPassword.Generate(NameLength);
			Text.SetText(randomText);
			await Task.Delay(20);
		}

		Text.SetText(Name);
	}
}
