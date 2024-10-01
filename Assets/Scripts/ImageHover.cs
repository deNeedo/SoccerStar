using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public Image description_box;
    public Text description_text;
    public void Start() {
        description_box.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData) {
        switch (gameObject.name) {
            case "Item 1":
                description_text.text = ItemManager.GetClothing(0).ToString(); break;
            case "Item 2":
                description_text.text = ItemManager.GetClothing(1).ToString(); break;
            case "Item 3":
                description_text.text = ItemManager.GetClothing(2).ToString(); break;
            case "Item 4":
                description_text.text = ItemManager.GetClothing(3).ToString(); break;
            case "Locker Item 1":
                if (PlayerManager.GetLockerItem(0) != null) {
                    description_text.text = PlayerManager.GetLockerItem(0).ToString(); 
                } break;
            case "Locker Item 2":
                if (PlayerManager.GetLockerItem(1) != null) {
                    description_text.text = PlayerManager.GetLockerItem(1).ToString(); 
                } break;
            case "Locker Item 3":
                if (PlayerManager.GetLockerItem(2) != null) {
                    description_text.text = PlayerManager.GetLockerItem(2).ToString(); 
                } break;
            case "Locker Item 4":
                if (PlayerManager.GetLockerItem(3) != null) {
                    description_text.text = PlayerManager.GetLockerItem(3).ToString(); 
                } break;
        }
        description_box.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        description_box.gameObject.SetActive(false);
        description_text.text = "";
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            int slot = -1;
            switch (gameObject.name) {
                case "Item 1":
                    slot = 0; break;
                case "Item 2":
                    slot = 1; break;
                case "Item 3":
                    slot = 2; break;
                case "Item 4":
                    slot = 3; break;
            }

            if (slot != -1) {
                Item itemToBuy = ItemManager.GetClothing(slot);

                if (PlayerManager.GetCash() >= itemToBuy.price) {
                    if (NetworkManager.BuyClothing(PlayerManager.GetName(), slot)) {
                        Item newItem = NetworkManager.GenerateClothing(PlayerManager.GetName(), slot);
                        ItemManager.SetClothing(newItem, slot);

                        NetworkManager.FetchLockerItems(PlayerManager.GetName());
                        NetworkManager.FetchCash(PlayerManager.GetName());
                        NetworkManager.FetchClothes(PlayerManager.GetName());
                        FindObjectOfType<LockerManager>().UpdateLockerUI(); 
                        FindObjectOfType<CashDisplayManager>().UpdateCashDisplay();
                        FindObjectOfType<ClothesManager>().UpdateShopItems();

                        Debug.Log("Purchased!");
                    } else {
                        Debug.Log("Failed to purchase the item.");
                    }
                } else {
                    Debug.Log("Not enough cash to purchase the item.");
                }
            }
        }
    }
}