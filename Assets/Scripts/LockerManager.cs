using UnityEngine;
using UnityEngine.UI;
public class LockerManager : MonoBehaviour {
    public Image lockerSlot1;
    public Image lockerSlot2;
    public Image lockerSlot3;
    public Image lockerSlot4;
    public Sprite empty;
    public Sprite blank;

    void Start() {
        lockerSlot1.gameObject.SetActive(false);
        lockerSlot2.gameObject.SetActive(false);
        lockerSlot3.gameObject.SetActive(false);
        lockerSlot4.gameObject.SetActive(false);

        UpdateLockerUI();
    }

    public void UpdateLockerUI() {
        UpdateLockerSlot(lockerSlot1, PlayerManager.GetLockerItem(0));
        UpdateLockerSlot(lockerSlot2, PlayerManager.GetLockerItem(1));
        UpdateLockerSlot(lockerSlot3, PlayerManager.GetLockerItem(2));
        UpdateLockerSlot(lockerSlot4, PlayerManager.GetLockerItem(3));
    }

    private void UpdateLockerSlot(Image lockerSlot, Item lockerItem) {
        if (lockerSlot == null) return;
        Image lockerItemImage = lockerSlot.transform.GetChild(0).GetComponent<Image>();

        if (lockerItemImage != null) {
            if (lockerItem == null || string.IsNullOrEmpty(lockerItem.name)) {
                lockerItemImage.sprite = blank;
            } else {
                lockerItemImage.sprite = GetSpriteForLockerItem(lockerItem);
            }
        }
    }

    public void LockerToggle() {
        lockerSlot1.gameObject.SetActive(!lockerSlot1.gameObject.activeSelf);
        lockerSlot2.gameObject.SetActive(!lockerSlot2.gameObject.activeSelf);
        lockerSlot3.gameObject.SetActive(!lockerSlot3.gameObject.activeSelf);
        lockerSlot4.gameObject.SetActive(!lockerSlot4.gameObject.activeSelf);
    }

    private Sprite GetSpriteForLockerItem(Item item) {
        if (item == null || string.IsNullOrEmpty(item.name)) {
            return empty;
        }

        Sprite itemSprite = ItemManager.GetSpriteForItem(item.name);
        return itemSprite ?? empty;
    }
}
