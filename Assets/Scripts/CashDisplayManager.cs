using UnityEngine;
using UnityEngine.UI;

public class CashDisplayManager : MonoBehaviour {
    public Text cashText;

    void Start() {
        UpdateCashDisplay();
    }

    public void UpdateCashDisplay() {
        double currentCash = PlayerManager.GetCash();
        cashText.text = $"Cash: {currentCash}";
    }
}
