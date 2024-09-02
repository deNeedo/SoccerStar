using UnityEngine;
using UnityEngine.UI;

public class EnduranceManager : MonoBehaviour
{
    public Slider enduranceSlider;
    public Text enduranceText;
    public Text feedbackText;
    public Button relaxButton;
    public Text buttonText;

    private int endurance;
    private int stars;
    private int relaxSessions;

    private void Start()
    {
        endurance = PlayerManager.GetEndurance();
        stars = PlayerManager.GetStars();
        relaxSessions = PlayerManager.GetRelaxSessions();

        enduranceSlider.value = endurance;
        UpdateEnduranceText();
        UpdateButtonText();
        UpdateFeedbackText("1 Star for Relax Session");

        // Add a listener to the button click
        relaxButton.onClick.AddListener(OnRelaxButtonClicked);
    }

    private void UpdateEnduranceText() {
        enduranceText.text = endurance.ToString();
    }
    private void UpdateFeedbackText(string message) {
        feedbackText.text = message;
    }
    private void UpdateButtonText() {
        buttonText.text = "Relax " + (10 - relaxSessions) + "/10";
    }

    private void OnRelaxButtonClicked() {
        if (stars > 0 && relaxSessions > 0 && endurance <= 80) {
            bool success = NetworkManager.UseRelaxSession(PlayerManager.Get());
            if (success) {
                endurance = PlayerManager.GetEndurance();
                stars = PlayerManager.GetStars();
                relaxSessions = PlayerManager.GetRelaxSessions();

                enduranceSlider.value = endurance;
                UpdateEnduranceText();
                UpdateButtonText();
                UpdateFeedbackText("Endurance increased!");
            }
            else {
                UpdateFeedbackText("Failed to use relax session. Please try again.");
            }
        }
        else {
            if (stars <= 0) {
                UpdateFeedbackText("Not enough stars to use relax session.");
            }
            else if (relaxSessions <= 0) {
                UpdateFeedbackText("Not enough relax sessions available.");
            }
            else if (endurance > 80) {
                UpdateFeedbackText("Endurance is already high. Cannot increase further.");
            }
        }
    }
}
