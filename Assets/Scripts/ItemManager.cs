using UnityEngine;
public class ItemManager : MonoBehaviour {
    private static readonly Item[] clothes = new Item[4];
    private static Item food;
    public static void RefillClothes() {
        for (int m = 0; m < 4; m++) {
            Item item = NetworkManager.GenerateClothing(PlayerManager.GetName(), m);
            SetClothing(item, m);
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
}