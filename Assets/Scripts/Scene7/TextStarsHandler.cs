// StarDisplayManager.cs
using UnityEngine;
using UnityEngine.UI;

public class StarDisplayManager : MonoBehaviour
{
    public Text starsText;

    private void OnEnable()
    {
        // Subscribe to the event when the script is enabled
        PlayerManager.OnStarsChanged.AddListener(UpdateStarsText);
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled to avoid memory leaks
        PlayerManager.OnStarsChanged.RemoveListener(UpdateStarsText);
    }

    private void Start()
    {
        UpdateStarsText(PlayerManager.GetStars());
    }

    private void UpdateStarsText(int stars)
    {
        starsText.text = "Stars: " + stars.ToString();
    }
}
