using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ImageHoverFood : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Image description_box;
    public Text description_text;
    public void Start() {
        description_box.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        description_text.text = ItemManager.GetFood().ToString();
        description_box.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description_box.gameObject.SetActive(false);
        description_text.text = "";
    }
}