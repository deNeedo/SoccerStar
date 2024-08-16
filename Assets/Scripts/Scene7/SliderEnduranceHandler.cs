using UnityEngine;
using UnityEngine.UI;

public class SliderEnduranceHandler : MonoBehaviour
{
    [SerializeField] private Slider enduranceSlider;
    [SerializeField] private Text enduranceText;

    private void Start()
    {
        UpdateSliderValue();
    }

    private void Update()
    {
        UpdateSliderValue();
    }

    public void UpdateSliderValue()
    {
        if (PlayerStats.Instance != null && enduranceSlider != null && enduranceText != null)
        {
            int currentEndurance = PlayerStats.Instance.GetCurrentEndurance();
            int initialEndurance = PlayerStats.Instance.GetInitialEndurance();
            enduranceSlider.value = (float)currentEndurance / initialEndurance;

            enduranceText.text = "Endurance: " + currentEndurance;
        }
    }
}
