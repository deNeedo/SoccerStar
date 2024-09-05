using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
public class WorkManager : MonoBehaviour
{
    public Text cashDisplayText;
    public Text sliderValueText;
    public Slider workSlider;
    public Button acceptButton;
    private bool isWorking = false;

    private void Start()
    {
        NetworkManager.CheckWorkCompletion();

        if (!string.IsNullOrEmpty(PlayerManager.GetEndTimeStr()) && !string.IsNullOrEmpty(PlayerManager.GetStartTimeStr()))
        {
            isWorking = true;
            acceptButton.GetComponentInChildren<Text>().text = "Cancel";
            Debug.Log("isWorking");
            workSlider.interactable = false;

            InvokeRepeating(nameof(UpdateWorkSliderWithRemainingTime), 0, 1f);
        }
        else 
        {
            Debug.Log("isn't Working");
            UpdateSliderText();
        }
        UpdateCashDisplay();
    }

    public void OnSliderValueChanged()
    {
        UpdateSliderText();
    }

    public void OnAcceptButtonClick()
    {
        if (isWorking)
        {
            if (NetworkManager.CancelWork() == true)
            {
                acceptButton.GetComponentInChildren<Text>().text = "Accept";
                workSlider.interactable = true;
                isWorking = false;

                CancelInvoke(nameof(UpdateWorkSliderWithRemainingTime));

                UpdateSliderText();
            }
            else
            {
                Debug.Log("Couldn't cancel");
            }
        }
        else
        {
            int hours = Mathf.RoundToInt(workSlider.value);
            Debug.Log("hours to work: " + hours);
            if (NetworkManager.StartWork(hours) == true)
            {
                acceptButton.GetComponentInChildren<Text>().text = "Cancel";
                workSlider.interactable = false;
                isWorking = true;
                NetworkManager.CheckWorkCompletion();
                InvokeRepeating(nameof(UpdateWorkSliderWithRemainingTime), 0, 1f);
            }
            else
            {
                Debug.Log("Couldn't start");
            }
        }
    }

    private void UpdateCashDisplay()
    {
        cashDisplayText.text = $"Cash: {PlayerManager.GetCash()}";
    }

    private void UpdateSliderText()
    {
        sliderValueText.text = "Hours: " + workSlider.value.ToString("F0");
    }

    private void UpdateWorkSliderWithRemainingTime()
    {
        string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        string startTimeStr = PlayerManager.GetStartTimeStr().Trim();
        string endTimeStr = PlayerManager.GetEndTimeStr().Trim();

        Debug.Log("Raw Start Time: " + startTimeStr);
        Debug.Log("Raw End Time: " + endTimeStr);

        if (DateTime.TryParseExact(endTimeStr, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endTime) &&
            DateTime.TryParseExact(startTimeStr, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
        {
            Debug.Log("Parsed End Time: " + endTime);
            Debug.Log("Parsed Start Time: " + startTime);

            DateTime currentTime = DateTime.Now;
            TimeSpan timeRemaining = endTime - currentTime;

            if (timeRemaining.TotalSeconds <= 0)
            {
                timeRemaining = TimeSpan.Zero;
                Debug.Log("Work time has ended.");
                CancelInvoke(nameof(UpdateWorkSliderWithRemainingTime));

                this.Start();
            }

            TimeSpan workShift = endTime - startTime;
            double totalWorkShiftMinutes = workShift.TotalMinutes;
            double remainingMinutes = timeRemaining.TotalMinutes;

            workSlider.value = (float)(remainingMinutes / totalWorkShiftMinutes) * 10;

            sliderValueText.text = $"Time left: {timeRemaining.Hours}h {timeRemaining.Minutes}m {timeRemaining.Seconds}s";
        }
        else
        {
            Debug.LogError("Failed to parse start or end time.");
        }
    }
}
