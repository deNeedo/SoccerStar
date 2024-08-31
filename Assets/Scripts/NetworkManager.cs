using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
public class NetworkManager : MonoBehaviour
{
    private static TcpClient client;
    private static NetworkStream stream;
    private static bool Connect() {
        try {
            NetworkManager.client = new TcpClient("10.147.17.201", 54429);
            NetworkManager.stream = client.GetStream();
            return true;
        } catch (Exception e) {
            Debug.LogError("Socket error: " + e.Message);
            return false;
        }
    }
    public static void Login(string username, string password) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "LOGIN " + username + " " + password + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);
        }
    }
    public static void Register(string username, string password) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "REGISTER " + username + " " + password + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);
        }
    }
    public static void Fetch(string username) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "FETCH " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);
        }
    }
    void Update() {
        if (stream != null && stream.DataAvailable) {
            byte[] receivedData = new byte[client.Available];
            NetworkManager.stream.Read(receivedData, 0, receivedData.Length);
            string message = Encoding.UTF8.GetString(receivedData);
            string[] temp = message.Split(' ');
            if (message.Contains("LOGIN 0") && GameManager.current_scene == "00_Login") {
                NetworkManager.Fetch(temp[2]);
                PlayerManager.Set(temp[2]);
                GameManager.ChangeScene("01_Profile");
            } else if (message.Contains("REGISTER 0") && GameManager.current_scene == "00_Register") {
                GameManager.ChangeScene("00_Login");
            } else if (message.Contains("FETCH 0")) {
                temp = temp[2].Split('\t');
                PlayerManager.Set(0, int.Parse(temp[0])); PlayerManager.Set(1, int.Parse(temp[1])); PlayerManager.Set(2, int.Parse(temp[2]));
            }
        }
    }
}