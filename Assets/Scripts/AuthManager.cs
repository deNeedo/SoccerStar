using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AuthManager : MonoBehaviour
{
    private string username;
    private string password;
    public InputField usernameField;
    public InputField passwordField;
    public void OnUsernameFieldValueChange(string newValue) {username = newValue;}
    public void OnPasswordFieldValueChange(string newValue) {password = newValue;}
    public string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int m = 0; m < bytes.Length; m++)
            {
                builder.Append(bytes[m].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    public void Validate()
    {
        if (username.Length >= 4 && username.Length <= 16 && password.Length >= 8)
        {
            password = HashPassword((username + password));
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "00_Login") {NetworkManager.Login(username, password);}
            else if (currentScene.name == "00_Register") {NetworkManager.Register(username, password);}            
        }
        else
        {
            Debug.Log("Username needs to be 4 to 16 characters long");
            Debug.Log("Password needs to be 8 or more characters long");
        }
    }
    public void Start()
    {
        usernameField.onValueChanged.AddListener(OnUsernameFieldValueChange);
        passwordField.onValueChanged.AddListener(OnPasswordFieldValueChange);
        username = usernameField.text; password = passwordField.text;
    }
}