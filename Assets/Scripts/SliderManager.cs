using UnityEngine;
using UnityEngine.UI;
public class SliderManager : MonoBehaviour
{
    public Slider slider0;
    public Slider slider1;
    public Slider slider2;
    public void Update() /* future rework requested */ { 
        slider0.maxValue = 100;
        slider0.value = PlayerManager.Get(0);
        slider1.maxValue = 100;
        slider1.value = PlayerManager.Get(1);
        slider2.maxValue = 100;
        slider2.value = PlayerManager.Get(2);
    }
}
