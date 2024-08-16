using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    private int _value1;
    public int Value1
    {
        get { return _value1; }
        set { _value1 = value; }
    }
    // public float Value2;
    // public float Value3;
    // public float Value1 {get {return _value1;} set {_value1 = value;}}
    // public float Value2 {get {return _value2;} set {_value2 = value;}}
    // public float Value3 {get {return _value3;} set {_value3 = value;}}
    void Awake()
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
    void Start()
    {
        Value1 = 10;
    }
}
