using UnityEngine;
public class ItemManager : MonoBehaviour {
    private static readonly Item[] clothes = new Item[4];
    private static Item food;
    private static Sprite[] allClothingSprites;
    void Awake() {
        allClothingSprites = Resources.LoadAll<Sprite>("clothes");
    }
    public static void RefillClothes() {
        for (int m = 0; m < 4; m++) {
            Item item = NetworkManager.GenerateClothing(PlayerManager.GetName(), m);
            SetClothing(item, m);
            FindObjectOfType<ClothesManager>().UpdateShopItems();
        }
    }
    public static void RefillFood() {
        SetFood(NetworkManager.GenerateFood(PlayerManager.GetName()));
    }
    public static void FetchClothes() {
        NetworkManager.FetchClothes(PlayerManager.GetName());
    }
    public static void FetchFood() {
        NetworkManager.FetchFood(PlayerManager.GetName());
    }
    public static void SetClothing(Item item, int slot) {
        clothes[slot] = item;
    }
    public static void SetFood(Item item) {
        food = item;
    }
    public static Item GetClothing(int slot) {
        return clothes[slot];
    }
    public static Item GetFood() {
        return food;
    }

    public static Sprite GetSpriteForItem(string itemName) {
        if (string.IsNullOrEmpty(itemName) || itemName.Length < 3) {
            Debug.LogWarning("Invalid item name. Cannot assign sprite.");
            return null;
        }

        string prefix = itemName.Substring(0, 1) + "00";
        foreach (Sprite sprite in allClothingSprites) {
            if (sprite.name.StartsWith(prefix)) {
                return sprite;
            }
        }

        Debug.LogWarning($"No sprite found for item with prefix {prefix}. Using default sprite.");

        return null;
    }

}