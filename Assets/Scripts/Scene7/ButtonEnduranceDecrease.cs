using UnityEngine;
using UnityEngine.UI;

public class EnduranceButtonHandler : MonoBehaviour
{
    [SerializeField] private Button decreaseButton;
    [SerializeField] private int decreaseAmount = 20;

    private void Start()
    {
        if (decreaseButton != null)
        {
            decreaseButton.onClick.AddListener(OnDecreaseButtonClicked);
        }
        else
        {
            Debug.LogError("Decrease Button is not assigned!");
        }
    }

    private void OnDecreaseButtonClicked()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.DecreaseEndurance(decreaseAmount);
        }
        else
        {
            Debug.LogError("PlayerStats instance is not found!");
        }
    }
}
