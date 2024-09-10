using UnityEngine;
using UnityEngine.UI;
using System;

public class TrainingManager : MonoBehaviour
{
    public Slider enduranceSlider;
    public Text descriptionText;
    public Button[] trainingOptionButtons;
    public Button acceptCancelButton;
    public Text sliderText;

    private int selectedOptionIndex = -1;
    private bool isTrainingActive = false;

    void Start()
    {
        NetworkManager.FetchTraining(PlayerManager.GetName());
        TrainingType currentTraining = PlayerManager.GetCurrentTraining();
        
        if (currentTraining != null)
        {
            isTrainingActive = true;
            SetAcceptCancelButtonText("Cancel");
            DisableTrainingOptionButtons();
            descriptionText.text = $"<size=26><b>{currentTraining.Title}</b></size>\n\n<size=22>{currentTraining.Description}</size>\n\nDuration: {currentTraining.Duration} minutes\nTrait Number: {currentTraining.Trait}";
        }
        else 
        {
            isTrainingActive = false;
            SetButtonsAndDescription();
            UpdateEnduranceDisplay();
            SetAcceptCancelButtonText("Accept");
            acceptCancelButton.interactable = false;
        }

        // Add listener for the accept/cancel button
        acceptCancelButton.onClick.AddListener(AcceptOrCancelTraining);
    }

    void Update()
    {
        if (isTrainingActive)
        {
            UpdateTrainingProgress();
        }
    }

    void SetButtonsAndDescription()
    {
        TrainingType[] availableTrainings = PlayerManager.GetAvailableTrainings();

        if (availableTrainings != null && availableTrainings.Length == trainingOptionButtons.Length)
        {
            for (int i = 0; i < trainingOptionButtons.Length; i++)
            {
                int index = i;
                Button button = trainingOptionButtons[i];
                button.GetComponentInChildren<Text>().text = availableTrainings[i].Title;

                // Clear existing listeners to avoid duplication
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => ShowOptionDescription(index, availableTrainings));
            }

            descriptionText.text = "Select a training option.";
        }
        else
        {
            Debug.LogError("No training options available or mismatch in button count.");
        }
    }

    void ShowOptionDescription(int index, TrainingType[] availableTrainings)
    {
        // Set the selected option index
        selectedOptionIndex = index;
        // Display the detailed description of the selected training option
        TrainingType selectedTraining = availableTrainings[index];
        string selectedOptionDescription = $"<size=26><b>{selectedTraining.Title}</b></size>\n\n<size=22>{selectedTraining.Description}</size>\n\nDuration: {selectedTraining.Duration} minutes\nTrait Number: {selectedTraining.Trait}";
        descriptionText.text = selectedOptionDescription;
        // Enable the accept button once a training option is selected
        acceptCancelButton.interactable = true;
    }

    public void AcceptOrCancelTraining()
    {
        if (isTrainingActive)
        {
            CancelTraining();
        }
        else if (selectedOptionIndex >= 0)
        {
            StartTraining();
        }
    }

    void StartTraining()
    {
        if (PlayerManager.IsWorking())
        {
            descriptionText.text = "You can't train while working!";
            Debug.LogError("Cannot start training: Player is currently working.");
            return;
        }

        TrainingType selectedTraining = PlayerManager.GetAvailableTrainings()[selectedOptionIndex];
        int traitNumber;
        if (int.TryParse(selectedTraining.Trait, out traitNumber))
        {
            int requiredEndurance = traitNumber * 10;
            if (PlayerManager.GetEndurance() < requiredEndurance)
            {
                descriptionText.text = $"Not enough endurance to start training. You need {requiredEndurance} endurance.";
                Debug.LogError("Cannot start training: Not enough endurance.");
                return;
            }
        }
        else
        {
            Debug.LogError("Invalid trait number format.");
            return;
        }

        bool trainingStarted = NetworkManager.StartTraining(PlayerManager.GetName(), selectedOptionIndex);
        if (trainingStarted)
        {
            NetworkManager.FetchTraining(PlayerManager.GetName());
            isTrainingActive = true;
            SetAcceptCancelButtonText("Cancel");
            DisableTrainingOptionButtons();
        }
        else
        {
            Debug.LogError("Failed to start training.");
        }
    }

    void CancelTraining()
    {
        bool trainingStopped = NetworkManager.StopTraining(PlayerManager.GetName());
        if (trainingStopped)
        {
            acceptCancelButton.interactable = false;
            NetworkManager.FetchEndurance(PlayerManager.GetName());
            PlayerManager.SetCurrentTraining(null);
            NetworkManager.FetchTraining(PlayerManager.GetName());
            isTrainingActive = false;
            SetAcceptCancelButtonText("Accept");
            UpdateEnduranceDisplay();
            SetButtonsAndDescription();
            EnableTrainingOptionButtons();
            Debug.Log("Training canceled.");
        }
        else
        {
            Debug.LogError("Failed to stop training.");
        }
    }

    void CompleteTraining()
    {
        PlayerManager.SetCurrentTraining(null);

        NetworkManager.FetchTraining(PlayerManager.GetName());
        SetButtonsAndDescription();
        NetworkManager.FetchTraits(PlayerManager.GetName());
        NetworkManager.FetchEndurance(PlayerManager.GetName());
        
        UpdateEnduranceDisplay();

        EnableTrainingOptionButtons();
        SetAcceptCancelButtonText("Accept");
        isTrainingActive = false;
        descriptionText.text = "Select a training option.";
    }

    private void UpdateTrainingProgress()
    {
        DateTime trainingEndTime;
        if (DateTime.TryParse(PlayerManager.GetCurrentTraining().TrainingEndTime, out trainingEndTime))
        {
            TimeSpan remainingTimeSpan = trainingEndTime - DateTime.Now;
            float remainingTime = (float)remainingTimeSpan.TotalSeconds;
            
            if (remainingTime > 0)
            {
                enduranceSlider.value = (1 - remainingTime / (60 * PlayerManager.GetCurrentTraining().Duration)) * 100;
                sliderText.text = $"Time remaining: {remainingTimeSpan.Minutes:00}:{remainingTimeSpan.Seconds:00}";
            }
            else
            {
                CompleteTraining();
            }
        }
        else
        {
            Debug.LogError("Invalid training end time format.");
        }
    }

    private void SetAcceptCancelButtonText(string text)
    {
        acceptCancelButton.GetComponentInChildren<Text>().text = text;
    }

    private void DisableTrainingOptionButtons()
    {
        foreach (Button button in trainingOptionButtons)
        {
            button.interactable = false;
        }
    }

    private void EnableTrainingOptionButtons()
    {
        foreach (Button button in trainingOptionButtons)
        {
            button.interactable = true;
        }
    }

    private void UpdateEnduranceDisplay()
    {
        enduranceSlider.value = PlayerManager.GetEndurance();
        sliderText.text = "Endurance: " + PlayerManager.GetEndurance() + "/100";
    }
}
