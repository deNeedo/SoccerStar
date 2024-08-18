using UnityEngine;
using UnityEngine.UI;

public class CashManager : MonoBehaviour
{
    [SerializeField] private Button resetCashButton;
    [SerializeField] private Text cashDisplayText;

    private void Start()
    {
        resetCashButton.onClick.AddListener(OnResetCashButtonClick);

        UpdateCashDisplay();
    }

    private void Update()
    {
        UpdateCashDisplay();
    }

    private void OnResetCashButtonClick()
    {
        PlayerStats.Instance.SetCash(0);
        UpdateCashDisplay();
        Debug.Log("Cash has been reset to 0.");
    }

    private void UpdateCashDisplay()
    {
        int currentCash = PlayerStats.Instance.GetCash();
        cashDisplayText.text = $"Cash: {currentCash}";
    }
}
