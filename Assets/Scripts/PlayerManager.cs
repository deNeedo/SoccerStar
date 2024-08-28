using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance {get; private set;}
    private string _player;
    public string Player {
        get {return _player;}
        set {_player = value;}
    }
    private int _fitness;
    public int Fitness {
        get {return _fitness;}
        set {_fitness = value;}
    }
    // define more traits in the same way
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    private void OnLoad() {
        // somehow fetch player info from server
        Player = NetworkManager.GetPlayer();
        Fitness = 10;
        // set all traits, items etc
        Debug.Log(Player);
    }
}
