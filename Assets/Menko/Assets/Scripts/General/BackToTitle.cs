using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour, IPointerClickHandler
{
    public RectTransform Left;
    public RectTransform Right;

    public RectTransform OptionTransform;
    public CanvasGroup OptionCanvas;

    public GameObject ContentObject;
    public GameObject SetMenkoObject;
    public GameObject PlayerPreviewObject;
    public GameObject CPUPreviewObject;
    public GameObject EndPreviewObject;

    private float initLeftX = -1575;
    private float initRightX = 1575;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            BackScene();
        }
    }

    private async void BackScene()
    {
        HideOutline();

        for (int i = 1; i <= 50; i++)
        {
            OptionZoomOut(i * -1.5f, 1 - i * 0.05f);

            initLeftX += 30;
            initRightX -= 30;

            Left.localPosition = new Vector3(initLeftX, 100, 0);
            Right.localPosition = new Vector3(initRightX, -100, 0);

            await Task.Delay(15);
        }

        SceneManager.LoadSceneAsync("Title");
    }

    private void HideOutline()
    {
        if (SetMenkoObject.transform.Find("Preview Menko(Clone)"))
        {
            GameObject PreviewObject = SetMenkoObject.transform.Find("Preview Menko(Clone)").gameObject;
            PreviewObject.GetComponent<Outline>().enabled = false;
        }

        for (int i = 0; i < ContentObject.transform.childCount; i++)
        {
            Transform transform = ContentObject.transform.GetChild(i);
            GameObject getPreviewObject = transform.Find("Preview Menko").gameObject;
            getPreviewObject.GetComponent<Outline>().enabled = false;
        }

        PlayerPreviewObject.GetComponent<Outline>().enabled = false;
        CPUPreviewObject.GetComponent<Outline>().enabled = false;
        EndPreviewObject.GetComponent<Outline>().enabled = false;
    }

    private void OptionZoomOut(float positionZ, float colorAlpha)
    {
        OptionTransform.localPosition = new Vector3(0, 0, positionZ);
        OptionCanvas.alpha = colorAlpha;
    }
}
