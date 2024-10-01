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
    private static double cash;
    private static string startWorkTimeStr;
    private static string endWorkTimeStr;
    private static TrainingType currentTraining = null;
    private static TrainingType[] availableTrainings = new TrainingType[3];
    private static Item[] locker = new Item[4];
    private static Item[] equiped = new Item[5];

    public static void SetName(string username) {
        PlayerManager.username = username;
    }
    public static string GetName() {
        return username;
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
    public static void SetStars(int stars) {
        PlayerManager.stars = stars;
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
    public static double GetCash() {
        return cash;
    }
    public static void SetCash(double newcash) {
        PlayerManager.cash = newcash;
    }
    public static string GetStartTimeStr() {
        return startWorkTimeStr;
    }
    public static void SetStartTimeStr(string newtime) {
        PlayerManager.startWorkTimeStr = newtime;
    }
    public static string GetEndTimeStr() {
        return endWorkTimeStr;
    }
    public static void SetEndTimeStr(string newtime) {
        PlayerManager.endWorkTimeStr = newtime;
    }
    public static void SetCurrentTraining(TrainingType currentTraining) {
        PlayerManager.currentTraining = currentTraining;
    }
    public static TrainingType GetCurrentTraining() {
        return currentTraining;
    }
    public static TrainingType[] GetAvailableTrainings() {
        return availableTrainings;
    }
    public static void SetAvailableTrainings(TrainingType[] trainings) {
        PlayerManager.availableTrainings = trainings;
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


    public static bool IsWorking() {
        return !string.IsNullOrEmpty(endWorkTimeStr);
    }
    public static bool IsTraining() {
        return currentTraining != null;
    }
}

public class TrainingType
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public float Duration { get; private set; }
    public string Trait { get; set; }
    public string TrainingEndTime { get; set; }
    public TrainingType(string title, string description, float duration)
    {
        Title = title;
        Description = description;
        Duration = duration;
    }
}
