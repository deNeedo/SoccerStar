using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField] private const int initialEndurance = 100;
    [SerializeField] private int currentEndurance;
    [SerializeField] private int relaxSessions = 0;
    [SerializeField] private int stars = 5;


    private const int MaxRelaxSessions = 10;

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
        //Relax
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
}
