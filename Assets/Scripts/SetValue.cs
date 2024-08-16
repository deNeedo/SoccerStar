using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetValue : MonoBehaviour
{
    public Slider Value1Slider;
    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            Value1Slider.maxValue = 100;
            Value1Slider.value = PlayerManager.Instance.Value1;
        }
    }
}
