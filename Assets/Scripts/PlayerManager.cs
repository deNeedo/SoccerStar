using UnityEngine;
public class PlayerManager : MonoBehaviour {
    private static string username = null;
    private static int trait0 = 0;
    private static int trait1 = 0;
    private static int trait2 = 0;

    private static int endurance;
    private static int relaxSessions;
    private static int stars;

    public static void Set(string username) {
        PlayerManager.username = username;
        Debug.Log("Set to: " + username);
    }
    public static void Set(int trait, int value) {
        switch (trait) {
            case 0:
                PlayerManager.trait0 = value;
                Debug.Log("Trait0 set to: " + value);
                break;
            case 1:
                PlayerManager.trait1 = value;
                Debug.Log("Trait1 set to: " + value);
                break;
            case 2:
                PlayerManager.trait2 = value;
                Debug.Log("Trait2 set to: " + value);
                break;
        }
    }
    public static void SetStars(int stars) {
        PlayerManager.stars = stars;
        Debug.Log("Set stars to: " + stars);
    }
    public static void SetEndurance(int endurance) {
        PlayerManager.endurance = endurance;
        Debug.Log("Set endurance to: " + endurance);
    }
    public static void SetSessions(int relaxSessions) {
        PlayerManager.relaxSessions = relaxSessions;
        Debug.Log("Set relaxSessions to: " + relaxSessions);
    }
    public static int Get(int trait) {
        switch (trait) {
            case 0:
                return trait0;
                break;
            case 1:
                return trait1;
                break;
            case 2:
                return trait2;
                break;
            default:
                return 0;
                break;
        }
    }
    public static string Get() {
        return username;
    }
    public static void Reset() {
        PlayerManager.username = null;
        Debug.Log("Reset");
    }
}
