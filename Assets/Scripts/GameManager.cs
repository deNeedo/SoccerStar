using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public InputField inputField;
    public void Start() {
        inputField.onValueChanged.AddListener(OnValueChange);
    }
    public static void OnValueChange(string newValue) {
        Debug.Log("Input Value Changed: " + newValue);
    }
    public static string HashPassword(string password) {
        using (SHA256 sha256 = SHA256.Create()) {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int m = 0; m < bytes.Length; m++) {
                builder.Append(bytes[m].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}