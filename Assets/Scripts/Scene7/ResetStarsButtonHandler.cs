using UnityEngine;
using UnityEngine.UI;

public class ResetStars : MonoBehaviour
{
    [SerializeField] private Button resetStarsButton;

    private void Start()
    {
        if (resetStarsButton != null)
        {
            resetStarsButton.onClick.AddListener(OnResetStarsButtonClicked);
        }
        else
        {
            Debug.LogError("Reset Stars Button is not assigned!");
        }
    }

    private void OnResetStarsButtonClicked()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.SetStars(5);
        }
        else
        {
            Debug.LogError("PlayerStats instance is not found!");
        }
    }
}
