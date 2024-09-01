using UnityEngine;
public class ItemManager : MonoBehaviour {
    public static Item[] store;
    public static Item[] locker;
    public static Item[] player;
    public static void Reset() {
        store = new Item[4];
        locker = new Item[4];
        player = new Item[5];
    }
}