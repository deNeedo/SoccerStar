using UnityEngine;
using UnityEngine.UI;

public class ButtonEnduranceIncrease : MonoBehaviour
{
    [SerializeField] private Button increaseButton;
    [SerializeField] private int increaseAmount = 20;

    private void Start()
    {
        if (increaseButton != null)
        {
            increaseButton.onClick.AddListener(OnIncreaseButtonClicked);
        }
        else
        {
            Debug.LogError("Increase Button is not assigned!");
        }
    }

    private void OnIncreaseButtonClicked()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.IncreaseEndurance(increaseAmount);
        }
        else
        {
            Debug.LogError("PlayerStats instance is not found!");
        }
    }
}
