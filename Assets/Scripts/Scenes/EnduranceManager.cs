using UnityEngine;
using UnityEngine.UI;

public class EnduranceManager : MonoBehaviour
{
    public Slider enduranceSlider;
    public Text enduranceText;
    public Text feedbackText;
    public Text buttonText;
    public Text starsText;
    public Button relaxButton;

    public void Start()
    {
        enduranceSlider.value = PlayerManager.GetEndurance();
        UpdateEnduranceText();
        UpdateButtonText();
        UpdateFeedbackText("1 Star for Relax Session");
        UpdateStarsText();

        if (PlayerManager.IsTraining() || PlayerManager.IsWorking())
        {
            relaxButton.interactable = false;
            UpdateFeedbackText("Cannot use relax session while training or working.");
        }
        else
        {
            relaxButton.interactable = true;
        }
    }

    private void UpdateEnduranceText()
    {
        enduranceText.text = PlayerManager.GetEndurance().ToString();
    }

    private void UpdateFeedbackText(string message)
    {
        feedbackText.text = message;
    }

    private void UpdateButtonText()
    {
        buttonText.text = "Relax " + (10 - PlayerManager.GetRelaxSessions()) + "/10";
    }

    private void UpdateStarsText()
    {
        starsText.text = "Stars: " + PlayerManager.GetStars();
    }

    public void OnRelaxButtonClicked()
    {
        if (PlayerManager.IsTraining() || PlayerManager.IsWorking())
        {
            UpdateFeedbackText("Cannot use relax session while training or working.");
            return;
        }

        if (PlayerManager.GetStars() > 0 && PlayerManager.GetRelaxSessions() > 0 && PlayerManager.GetEndurance() <= 80)
        {
            bool success = NetworkManager.UseRelaxSession(PlayerManager.GetName());
            if (success)
            {
                enduranceSlider.value = PlayerManager.GetEndurance();
                UpdateEnduranceText();
                UpdateButtonText();
                UpdateFeedbackText("Endurance increased!");
                UpdateStarsText();
            }
            else
            {
                UpdateFeedbackText("Failed to use relax session. Please try again.");
            }
        }
        else
        {
            if (PlayerManager.GetStars() <= 0)
            {
                UpdateFeedbackText("Not enough stars to use relax session.");
            }
            else if (PlayerManager.GetRelaxSessions() <= 0)
            {
                UpdateFeedbackText("Not enough relax sessions available.");
            }
            else if (PlayerManager.GetEndurance() > 80)
            {
                UpdateFeedbackText("Endurance is already high. Cannot increase further.");
            }
        }
    }
}
