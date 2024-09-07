using UnityEngine;
using UnityEngine.EventSystems;
public class ImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered: " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited: " + gameObject.name);
    }
}