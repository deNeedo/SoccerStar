using UnityEngine;
using UnityEngine.UI;

public class TextStarsHandler : MonoBehaviour
{
    [SerializeField] private Text starsText;

    private void Start()
    {
        if (starsText == null)
        {
            Debug.LogError("Stars Text is not assigned!");
            return;
        }

        UpdateStarsText();
    }

    private void Update()
    {
        UpdateStarsText();
    }

    public void UpdateStarsText()
    {
        if (PlayerStats.Instance != null)
        {
            int stars = PlayerStats.Instance.GetStars();
            starsText.text = "Stars: " + stars;
        }
        else
        {
            Debug.LogError("PlayerStats instance is not found!");
        }
    }
}
