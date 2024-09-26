using UnityEngine;
using UnityEngine.UI;
public class ClothesManager : MonoBehaviour {
    public Image shop_item_1;
    public Image shop_item_2;
    public Image shop_item_3;
    public Image shop_item_4;
    public Sprite empty;
    public void Start() {
        UpdateShopItems();
    }
    public void UpdateShopItems() {
        UpdateShopItem(shop_item_1, ItemManager.GetClothing(0));
        UpdateShopItem(shop_item_2, ItemManager.GetClothing(1));
        UpdateShopItem(shop_item_3, ItemManager.GetClothing(2));
        UpdateShopItem(shop_item_4, ItemManager.GetClothing(3));
    }
    private void UpdateShopItem(Image shopItemImage, Item item) {
        if (item != null) {
            Sprite itemSprite = ItemManager.GetSpriteForItem(item.name);
            shopItemImage.sprite = itemSprite != null ? itemSprite : empty;
        } else {
            shopItemImage.sprite = empty;
        }
    }
}