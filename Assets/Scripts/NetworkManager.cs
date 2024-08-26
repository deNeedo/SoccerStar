using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NetworkManager : MonoBehaviour
{
    private static TcpClient client;
    private static NetworkStream stream;
    private static bool Connect()
    {
        try
        {
            NetworkManager.client = new TcpClient("10.147.17.201", 54429);
            NetworkManager.stream = client.GetStream();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Socket error: " + e.Message);
            return false;
        }
    }
    public static void Login(string username, string password)
    {
        bool flag = NetworkManager.Connect();
        if (flag == true)
        {
            string message = "LOGIN " + username + " " + password + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);
        }
    }
    public static void Register(string username, string password)
    {
        bool flag = NetworkManager.Connect();
        if (flag == true)
        {
            string message = "REGISTER " + username + " " + password + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);
        }
    }
    void Update()
    {
        if (stream != null && stream.DataAvailable)
        {
            byte[] receivedData = new byte[client.Available];
            NetworkManager.stream.Read(receivedData, 0, receivedData.Length);
            string message = Encoding.UTF8.GetString(receivedData);
            Scene currentScene = SceneManager.GetActiveScene();
            Debug.Log(message);
            if (message.Contains("0") && currentScene.name == "00_Login")
            {
                ChangeScreen.ChangeScene("01_Profile");
            }
            else if (message.Contains("0") && currentScene.name == "00_Register")
            {
                ChangeScreen.ChangeScene("00_Login");
            }
        }
    }
}