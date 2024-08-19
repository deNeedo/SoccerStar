using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance {get; private set;}
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
    private void Start() {
        // somehow fetch player info from server
        Fitness = 10;
        // set all traits, items etc
    }
}
