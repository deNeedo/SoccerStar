using UnityEngine;
using UnityEngine.Events;
public class PlayerManager : MonoBehaviour {
    private static string username = null;
    private static int trait0 = 0;
    private static int trait1 = 0;
    private static int trait2 = 0;
    private static int endurance;
    private static int relaxSessions;
    private static int stars;
    public static UnityEvent<int> OnStarsChanged = new UnityEvent<int>();

    public static void SetName(string username) {
        PlayerManager.username = username;
    }
    public static void SetTrait(int trait, int value) {
        switch (trait) {
            case 0:
                PlayerManager.trait0 = value;
                break;
            case 1:
                PlayerManager.trait1 = value;
                break;
            case 2:
                PlayerManager.trait2 = value;
                break;
        }
    }
    public static void SetStars(int stars) {
        PlayerManager.stars = stars;
        OnStarsChanged.Invoke(stars);
    }
    public static int GetStars() {
        return PlayerManager.stars;
    }
    public static void SetEndurance(int endurance) {
        PlayerManager.endurance = endurance;
    }
    public static int GetEndurance() {
        return PlayerManager.endurance;
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
        PlayerManager.username = null;
    }
}
