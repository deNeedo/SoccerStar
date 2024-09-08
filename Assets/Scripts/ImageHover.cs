using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public Image description_box;
    public Text description_text;
    public void Start() {
        description_box.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (gameObject.name) {
            case "Item 1":
                description_text.text = ItemManager.GetClothing(0).ToString(); break;
            case "Item 2":
                description_text.text = ItemManager.GetClothing(1).ToString(); break;
            case "Item 3":
                description_text.text = ItemManager.GetClothing(2).ToString(); break;
            case "Item 4":
                description_text.text = ItemManager.GetClothing(3).ToString(); break;
        }
        description_box.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description_box.gameObject.SetActive(false);
        description_text.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) {
            Debug.Log("Purchased!");
        }
    }
}