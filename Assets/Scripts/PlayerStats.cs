using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField] private int currentEndurance;
    [SerializeField] private int relaxSessions = 0;

    [SerializeField] private int stars = 5;
    [SerializeField] private int cash = 0;

    [SerializeField] private long workStartTimestamp = 0;
    [SerializeField] private long workEndTimestamp = 0;

    private const int initialEndurance = 100;
    private const int MaxRelaxSessions = 10;
    private const int MaxTimeToEndOfWorkInSeconds = 3600 * 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        //Endurance
        if (PlayerPrefs.HasKey("CurrentEndurance"))
        {
            currentEndurance = PlayerPrefs.GetInt("CurrentEndurance");
        }
        else
        {
            currentEndurance = initialEndurance;
            PlayerPrefs.SetInt("CurrentEndurance", currentEndurance);
        }
        //Relax Sessions
        if (PlayerPrefs.HasKey("RelaxSessions"))
        {
            relaxSessions = PlayerPrefs.GetInt("RelaxSessions");
        }
        else
        {
            relaxSessions = 0;
            PlayerPrefs.SetInt("RelaxSessions", relaxSessions);
        }
        //Stars
        if (PlayerPrefs.HasKey("Stars"))
        {
            stars = PlayerPrefs.GetInt("Stars");
        }
        else
        {
            stars = 5;
            PlayerPrefs.SetInt("Stars", stars);
        }
        // Cash
        if (PlayerPrefs.HasKey("Cash"))
        {
            cash = PlayerPrefs.GetInt("Cash");
        }
        else
        {
            cash = 0;
            PlayerPrefs.SetInt("Cash", cash);
        }
        // Time When Work Started and Ends
        if (PlayerPrefs.HasKey("WorkStartTimestamp") && PlayerPrefs.HasKey("WorkEndTimestamp"))
        {
            workStartTimestamp = PlayerPrefs.GetInt("WorkStartTimestamp");
            workEndTimestamp = PlayerPrefs.GetInt("WorkEndTimestamp");
        }
        else
        {
            workStartTimestamp = 0;
            workEndTimestamp = 0;
            PlayerPrefs.SetInt("WorkStartTimestamp", (int)workStartTimestamp);
            PlayerPrefs.SetFloat("WorkEndTimestamp", (int)workEndTimestamp);
        }
    }

    public void DecreaseEndurance(int amount)
    {
        currentEndurance = currentEndurance - amount;
        if (currentEndurance < 0)
        {
            Debug.Log("Endurance should not be negative.");
            currentEndurance = currentEndurance + amount;
        }
        PlayerPrefs.SetInt("CurrentEndurance", currentEndurance);
    }

    public void IncreaseEndurance(int amount)
    {
        currentEndurance = currentEndurance + amount;
        if (currentEndurance > initialEndurance)
        {
            Debug.Log("Endurance should not be > 100.");
            currentEndurance = currentEndurance - amount;
        }
        PlayerPrefs.SetInt("CurrentEndurance", currentEndurance);
    }

    public void IncreaseRelaxSessions()
    {
        if (relaxSessions < MaxRelaxSessions)
        {
            relaxSessions++;
            PlayerPrefs.SetInt("RelaxSessions", relaxSessions);
        }
    }

    public void DecreaseStars(int amount)
    {
        stars = stars - amount;
        if (stars < 0)
        {
            Debug.Log("Stars should not be < 0.");
            stars = stars + amount;
        }
        PlayerPrefs.SetInt("Stars", stars);
    }

    public void IncreaseCash(int amount)
    {
        cash += amount;
        PlayerPrefs.SetInt("Cash", cash);
    }

    public void DecreaseCash(int amount)
    {
        cash -= amount;
        if (cash < 0)
        {
            Debug.LogError("Cash should not be negative.");
            cash += amount;
        }
        PlayerPrefs.SetInt("Cash", cash);
    }

    public void SetCash(int newCash)
    {
        if (newCash >= 0)
        {
            cash = newCash;
            PlayerPrefs.SetInt("Cash", cash);
        }
    }

    public void SetEndTime(DateTime endTime)
    {
        workEndTimestamp = ((DateTimeOffset)endTime).ToUnixTimeSeconds();
        PlayerPrefs.SetInt("WorkEndTimestamp", (int)workEndTimestamp);
    }

    public void SetStartTime(DateTime startTime)
    {
        workStartTimestamp = ((DateTimeOffset)startTime).ToUnixTimeSeconds();
        PlayerPrefs.SetInt("WorkStartTimestamp", (int)workStartTimestamp);
    }

    public DateTime GetEndTime()
    {
        return DateTimeOffset.FromUnixTimeSeconds(workEndTimestamp).DateTime;
    }

    public DateTime GetStartTime()
    {
        return DateTimeOffset.FromUnixTimeSeconds(workStartTimestamp).DateTime;
    }

    public int GetCurrentEndurance()
    {
        return currentEndurance;
    }

    public int GetInitialEndurance()
    {
        return initialEndurance;
    }

    public int GetRelaxSessions()
    {
        return relaxSessions;
    }

    public void ResetRelaxSessions()
    {
        relaxSessions = 0;
        PlayerPrefs.SetInt("RelaxSessions", relaxSessions);
    }

    public int GetStars()
    {
        return stars;
    }

    public void SetStars(int newStars)
    {
        if (newStars >= 0)
        {
            stars = newStars;
            PlayerPrefs.SetInt("Stars", stars);
        }
    }

    public bool IsWorkSessionActive()
    {
        long currentTimestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
        return workEndTimestamp > currentTimestamp;
    }

    public int GetCash()
    {
        return cash;
    }
}
