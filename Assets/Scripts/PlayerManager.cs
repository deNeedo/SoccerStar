using UnityEngine;
using UnityEngine.Events;
public class PlayerManager : MonoBehaviour {
    private static string username = null;
    private static int trait0 = 0;
    private static int trait1 = 0;
    private static int trait2 = 0;

    private static double cash;
    private static int endurance;
    private static int relaxSessions;
    private static int stars;
    private static string startTimeStr;
    private static string endTimeStr;
    private static float trainingDuration;
    private static float trainingEndTime;


    public static UnityEvent<int> OnStarsChanged = new UnityEvent<int>();

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
        OnStarsChanged.Invoke(stars);
        Debug.Log("Set stars to: " + stars);
    }
    public static int GetStars() {
        return PlayerManager.stars;
    }

    public static void SetEndurance(int endurance) {
        PlayerManager.endurance = endurance;
        Debug.Log("Set endurance to: " + endurance);
    }
    public static int GetEndurance() {
        return PlayerManager.endurance;
    }
    public static void SetSessions(int relaxSessions) {
        PlayerManager.relaxSessions = relaxSessions;
        Debug.Log("Set relaxSessions to: " + relaxSessions);
    }
    public static int GetRelaxSessions() {
        return relaxSessions;
    }
    public static double GetCash() {
        return cash;
    }
    public static void SetCash(double newcash) {
        PlayerManager.cash = newcash;
    }
    public static string GetStartTimeStr() {
        return startTimeStr;
    }
    public static void SetStartTimeStr(string newtime) {
        PlayerManager.startTimeStr = newtime;
    }
    public static string GetEndTimeStr() {
        return endTimeStr;
    }
    public static void SetEndTimeStr(string newtime) {
        PlayerManager.endTimeStr = newtime;
    }

    public static int Get(int trait) {
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
    public static string Get() {
        return username;
    }
    public static void Reset() {
        PlayerManager.username = null;
        Debug.Log("Reset");
    }
    public static bool IsWorking() {
        return !string.IsNullOrEmpty(endTimeStr);
    }

}
