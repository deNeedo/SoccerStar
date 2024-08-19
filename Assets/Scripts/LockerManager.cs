using UnityEngine;
using UnityEngine.UI;

public class LockerManager : MonoBehaviour {
    public Image lockerSlot1;
    public Image lockerSlot2;
    public Image lockerSlot3;
    public Image lockerSlot4;

    public void LockerToggle()
    {
        lockerSlot1.gameObject.SetActive(!lockerSlot1.gameObject.activeSelf);
        lockerSlot2.gameObject.SetActive(!lockerSlot2.gameObject.activeSelf);
        lockerSlot3.gameObject.SetActive(!lockerSlot3.gameObject.activeSelf);
        lockerSlot4.gameObject.SetActive(!lockerSlot4.gameObject.activeSelf);
    }
    void Start()
    {
        lockerSlot1.gameObject.SetActive(false);
        lockerSlot2.gameObject.SetActive(false);
        lockerSlot3.gameObject.SetActive(false);
        lockerSlot4.gameObject.SetActive(false);
    }
}