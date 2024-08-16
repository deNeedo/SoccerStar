using UnityEngine;
using UnityEngine.UI;

public class ButtonSessionReset : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private Text buttonText;

    private void Start()
    {
        if (resetButton == null || buttonText == null)
        {
            Debug.LogError("Button or Text component is not assigned!");
            return;
        }

        resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    private void UpdateButtonText()
    {
        if (PlayerStats.Instance != null)
        {
            int relaxSessions = PlayerStats.Instance.GetRelaxSessions();
            buttonText.text = $"{relaxSessions}/10";
        }
    }

    private void OnResetButtonClicked()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetRelaxSessions();
            UpdateButtonText();
        }
        else
        {
            Debug.LogError("PlayerStats instance is not found!");
        }
    }
}
