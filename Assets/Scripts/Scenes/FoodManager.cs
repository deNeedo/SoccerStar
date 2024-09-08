using UnityEngine;
using UnityEngine.UI;
public class FoodManager : MonoBehaviour {
    public Image shop_item;
    public Sprite empty;
    public Sprite item_1;
    public void Start() {
        if (ItemManager.GetFood() != null) {
            shop_item.sprite = item_1;
        } else {
            shop_item.sprite = empty;
        }
    }
}