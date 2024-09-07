// using System.Collections;
// using UnityEngine;
// using UnityEngine.UI;
// using System;

// public class WorkManager : MonoBehaviour
// {
//     [SerializeField] private Button acceptButton;
//     [SerializeField] private Button cancelButton;
//     [SerializeField] private Button testCompleteButton;
//     [SerializeField] private Slider workSlider;
//     [SerializeField] private Text hoursText;
//     [SerializeField] private Text timeRemainingText;

//     private Coroutine updateCoroutine;

//     private void Start()
//     {
//         workSlider.minValue = 1;
//         workSlider.maxValue = 10;
//         workSlider.value = 1;

//         workSlider.onValueChanged.AddListener(OnSliderValueChanged);
//         acceptButton.onClick.AddListener(OnAcceptButtonClick);
//         cancelButton.onClick.AddListener(OnCancelButtonClick);
//         testCompleteButton.onClick.AddListener(OnTestCompleteButtonClick);

//         CheckInitialWorkSessionStatus();
//     }

//     private void OnSliderValueChanged(float value)
//     {
//         hoursText.text = $"Hours: {Mathf.RoundToInt(value)}";
//     }

//     private void OnAcceptButtonClick()
//     {
//         int hours = Mathf.RoundToInt(workSlider.value);
//         DateTime endTime = DateTime.UtcNow.AddHours(hours);

//         PlayerStats.Instance.SetEndTime(endTime);
//         PlayerStats.Instance.SetStartTime(DateTime.UtcNow);

//         Debug.Log($"Work session started for {hours} hours.");

//         SetSliderInteractable(false);

//         if (updateCoroutine != null)
//         {
//             StopCoroutine(updateCoroutine);
//         }
//         updateCoroutine = StartCoroutine(UpdateTimeRemaining());
//     }

//     private void OnTestCompleteButtonClick()
//     {
//         // Immediately complete the work session
//         CompleteWorkSession();
//     }

//     private void CheckInitialWorkSessionStatus()
//     {
//         if (PlayerStats.Instance.IsWorkSessionActive())
//         {
//             SetSliderInteractable(false);

//             DateTime endTime = PlayerStats.Instance.GetEndTime();
//             DateTime startTime = PlayerStats.Instance.GetStartTime();

//             if (endTime > DateTime.UtcNow)
//             {
//                 TimeSpan remainingTime = endTime - DateTime.UtcNow;

//                 float remainingHours = (float)remainingTime.TotalHours;
//                 workSlider.value = remainingHours;

//                 timeRemainingText.text = $"Time Remaining: {FormatTimeSpan(remainingTime)}";

//                 updateCoroutine = StartCoroutine(UpdateTimeRemaining());
//             }
//             else
//             {
//                 CompleteWorkSession();
//             }
//         }
//         else
//         {
//             timeRemainingText.text = "No active work session.";
//             SetSliderInteractable(true);
//         }
//     }

//     private IEnumerator UpdateTimeRemaining()
//     {
//         while (PlayerStats.Instance.IsWorkSessionActive())
//         {
//             DateTime endTime = PlayerStats.Instance.GetEndTime();
//             DateTime startTime = PlayerStats.Instance.GetStartTime();

//             if (endTime > DateTime.UtcNow)
//             {
//                 TimeSpan remainingTime = endTime - DateTime.UtcNow;
//                 timeRemainingText.text = $"Time Remaining: {FormatTimeSpan(remainingTime)}";
//             }
//             else
//             {
//                 CompleteWorkSession();
//                 yield break;
//             }

//             yield return new WaitForSeconds(1);
//         }
//     }

//     private void CompleteWorkSession()
//     {
//         DateTime endTime = PlayerStats.Instance.GetEndTime();
//         DateTime startTime = PlayerStats.Instance.GetStartTime();
//         TimeSpan workedDuration = endTime - startTime;
// //        Debug.Log($"Got {workedDuration} workedDuration!");
// //        Debug.Log($"Got {workedDuration.TotalHours} workedDuration.TotalHours!");

//         int hoursWorked = Mathf.RoundToInt((float)workedDuration.TotalHours);
//         int cashEarned = hoursWorked * 100;
//         PlayerStats.Instance.IncreaseCash(cashEarned);

//         Debug.Log($"Got {cashEarned} cash!");
//         timeRemainingText.text = "Work session completed!";

//         PlayerStats.Instance.SetEndTime(DateTimeOffset.FromUnixTimeSeconds(0).DateTime);
//         PlayerStats.Instance.SetStartTime(DateTimeOffset.FromUnixTimeSeconds(0).DateTime);

//         SetSliderInteractable(true);
//     }

//     private void OnCancelButtonClick()
//     {
//         PlayerStats.Instance.SetEndTime(DateTimeOffset.FromUnixTimeSeconds(0).DateTime);
//         PlayerStats.Instance.SetStartTime(DateTimeOffset.FromUnixTimeSeconds(0).DateTime);

//         SetSliderInteractable(true);

//         timeRemainingText.text = "Work session canceled.";

//         if (updateCoroutine != null)
//         {
//             StopCoroutine(updateCoroutine);
//             updateCoroutine = null;
//         }
//     }

//     private void SetSliderInteractable(bool interactable)
//     {
//         workSlider.interactable = interactable;
//         acceptButton.interactable = interactable;
// //        Debug.Log($"Got {interactable} interactable!");
//     }

//     private string FormatTimeSpan(TimeSpan timeSpan)
//     {
//         return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
//     }
// }
