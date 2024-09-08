using UnityEngine;
using UnityEngine.UI;
public class ClothesManager : MonoBehaviour {
    public Image shop_item_1;
    public Image shop_item_2;
    public Image shop_item_3;
    public Image shop_item_4;
    public Sprite empty;
    public Sprite item_1;
    public void Start() {
        for (int m = 0; m < 4; m++) {
            if (ItemManager.GetClothing(m) != null) {
                switch (m) {
                    case 0:
                        shop_item_1.sprite = item_1; break;
                    case 1:
                        shop_item_2.sprite = item_1; break;
                    case 2:
                        shop_item_3.sprite = item_1; break;
                    case 3:
                        shop_item_4.sprite = item_1; break;
                }
            } else {
                shop_item_1.sprite = empty;
            }
        }
    }
}