using UnityEngine;
using UnityEngine.UI;

public class RelaxSessionsButtonHandler : MonoBehaviour
{
    [SerializeField] private Button relaxSessionsButton;
    [SerializeField] private Text buttonText;

    private const int EnduranceMaxValue = 100;
    private const int EnduranceIncreaseAmount = 20;

    private void Start()
    {
        if (relaxSessionsButton == null || buttonText == null)
        {
            Debug.LogError("Button or Text component is not assigned!");
            return;
        }

        UpdateButtonText();

        relaxSessionsButton.onClick.AddListener(OnButtonClicked);
    }

    private void UpdateButtonText()
    {
        if (PlayerStats.Instance != null)
        {
            int relaxSessions = PlayerStats.Instance.GetRelaxSessions();
            buttonText.text = $"Available Sessions: {relaxSessions}/10";
        }
    }

    private void OnButtonClicked()
    {
        if (PlayerStats.Instance == null)
        {
            Debug.LogError("PlayerStats instance is not found!");
        }
        else if (PlayerStats.Instance.GetCurrentEndurance() > EnduranceMaxValue - EnduranceIncreaseAmount)
        {
            Debug.Log("Max Endurance");
        }
        else if (PlayerStats.Instance.GetRelaxSessions() >= 10)
        {
            Debug.Log("All Sessions Used");
        }
        else if (PlayerStats.Instance.GetStars() <= 0)
        {
            Debug.Log("Not Enough Stars");
        }
        else
        {
            PlayerStats.Instance.DecreaseStars(1);
            PlayerStats.Instance.IncreaseEndurance(EnduranceIncreaseAmount);
            PlayerStats.Instance.IncreaseRelaxSessions();
            UpdateButtonText();
        }
    }
}
