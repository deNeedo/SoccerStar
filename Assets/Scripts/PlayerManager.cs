using UnityEngine;
using UnityEngine.Events;
public class PlayerManager : MonoBehaviour {
    private static string username;
    private static int trait0;
    private static int trait1;
    private static int trait2;
    private static int endurance;
    private static int relaxSessions;
    private static int stars;
    private static Item[] locker = new Item[4];
    private static Item[] equiped = new Item[5];
    public static UnityEvent<int> OnStarsChanged = new UnityEvent<int>();

    public static void SetName(string username) {
        PlayerManager.username = username;
    }
    public static void SetTrait(int trait, int value) {
        switch (trait) {
            case 0:
                trait0 = value;
                break;
            case 1:
                trait1 = value;
                break;
            case 2:
                trait2 = value;
                break;
        }
    }
    public static void SetStars(int stars) {
        PlayerManager.stars = stars;
        OnStarsChanged.Invoke(stars);
    }
    public static int GetStars() {
        return stars;
    }
    public static void SetEndurance(int endurance) {
        PlayerManager.endurance = endurance;
    }
    public static int GetEndurance() {
        return endurance;
    }
    public static void SetSessions(int relaxSessions) {
        PlayerManager.relaxSessions = relaxSessions;
    }
    public static int GetRelaxSessions() {
        return relaxSessions;
    }
    public static int GetTrait(int trait) {
        switch (trait) {
            case 0:
                return trait0;
            case 1:
                return trait1;
            case 2:
                return trait2;
            default:
                return 0;
        }
    }
    public static string GetName() {
        return username;
    }
    public static void Reset() {
        username = null;
    }
    public static void SetLockerItem(int place, Item item) {
        locker[place] = item;
    }
    public static Item GetLockerItem(int place) {
        return locker[place];
    }
}
