using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static TcpClient client;
    private static NetworkStream stream;
    private static string configFileName = "App.config";
    private static string server_ip;
    private static int server_port;
    private static string server_response = null;
    private static void ResponseCheck() {
        if (stream != null && stream.DataAvailable) {
            byte[] receivedData = new byte[client.Available];
            NetworkManager.stream.Read(receivedData, 0, receivedData.Length);
            NetworkManager.server_response = Encoding.UTF8.GetString(receivedData);
        }
    }
    private static void ConnectInit() {
        string path = Path.Combine(Application.streamingAssetsPath, configFileName);
        if (File.Exists(path)) {
            try {
                XmlDocument xmlDoc = new XmlDocument(); xmlDoc.Load(path);
                XmlNodeList nodeList = xmlDoc.SelectNodes("/configuration/appSettings/add");
                foreach (XmlNode node in nodeList) {
                    string key = node.Attributes["key"].Value;
                    string value = node.Attributes["value"].Value;
                    if (key == "ServerIP") {server_ip = value;}
                    else if (key == "ServerPort") {server_port = int.Parse(value);}
                }
            } catch (Exception e) {Debug.LogError("Error reading App.config: " + e.Message);}
        } else {Debug.LogError("App.config file not found!");}
    } 
    private static bool Connect() {
        try {
            if (NetworkManager.server_ip == null) {NetworkManager.ConnectInit();}
            NetworkManager.client = new TcpClient(server_ip, server_port);
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
            while (NetworkManager.server_response == null) {NetworkManager.ResponseCheck(); Thread.Sleep(10);}
            string[] temp = (NetworkManager.server_response).Split(' ');
            NetworkManager.server_response = null;
            if (temp[1] == "0") {
                NetworkManager.Fetch(temp[2]);
                NetworkManager.FetchStars(temp[2]);
                NetworkManager.FetchEndurance(temp[2]);
                NetworkManager.FetchSessions(temp[2]);
                PlayerManager.Set(temp[2]);
                ItemManager.Reset();
                GameManager.ChangeScene("01_Profile");
            }
        }
    }
    public static void Register(string username, string password) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "REGISTER " + username + " " + password + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);
            while (NetworkManager.server_response == null) {NetworkManager.ResponseCheck(); Thread.Sleep(10);}
            string[] temp = (NetworkManager.server_response).Split(' ');
            NetworkManager.server_response = null;
            if (temp[1] == "0") {
                GameManager.ChangeScene("00_Login");
            }
        }
    }
    public static void Fetch(string username) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "FETCHSTATS " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);
            while (NetworkManager.server_response == null) {NetworkManager.ResponseCheck(); Thread.Sleep(10);}
            string[] temp = (NetworkManager.server_response).Split(' ');
            NetworkManager.server_response = null;
            if (temp[1] == "0") {
                temp = temp[2].Split('\t');
                PlayerManager.Set(0, int.Parse(temp[0])); PlayerManager.Set(1, int.Parse(temp[1])); PlayerManager.Set(2, int.Parse(temp[2]));
            }
        }
    }
    public static void FetchStars(string username) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "FETCHSTARS " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.WriteAsync(data, 0, data.Length);
            while (NetworkManager.server_response == null) {NetworkManager.ResponseCheck(); Thread.Sleep(10);}
            string[] temp = (NetworkManager.server_response).Split(' ');
            NetworkManager.server_response = null;
            if (temp[1] == "0") {
                PlayerManager.SetStars(int.Parse(temp[2]));
            }
        }
    }
    public static void FetchEndurance(string username) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "FETCHENDURANCE " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.WriteAsync(data, 0, data.Length);
            while (NetworkManager.server_response == null) {NetworkManager.ResponseCheck(); Thread.Sleep(10);}
            string[] temp = (NetworkManager.server_response).Split(' ');
            NetworkManager.server_response = null;
            if (temp[1] == "0") {
                PlayerManager.SetEndurance(int.Parse(temp[2]));
            }
        }
    }
    public static void FetchSessions(string username) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "FETCHSESSIONS " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.WriteAsync(data, 0, data.Length);
            while (NetworkManager.server_response == null) {NetworkManager.ResponseCheck(); Thread.Sleep(10);}
            string[] temp = (NetworkManager.server_response).Split(' ');
            NetworkManager.server_response = null;
            if (temp[1] == "0") {
                PlayerManager.SetSessions(int.Parse(temp[2]));
            }
        }
    }
    public static void GenerateItem(string username) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "CREATEITEM " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);
            while (NetworkManager.server_response == null) {NetworkManager.ResponseCheck(); Thread.Sleep(10);}
            string[] temp = (NetworkManager.server_response).Split(' ');
            NetworkManager.server_response = null;
            temp = temp[2].Split('\t');
            string name = temp[0]; int trait = int.Parse(temp[1].Split(',')[0]); int value = int.Parse(temp[1].Split(',')[1]);
            if (trait == 0) {
                ItemManager.locker[0] = new Item(name, value, 0 , 0);
            } else if (trait == 1) {
                ItemManager.locker[0] = new Item(name, 0, value , 0);
            } else if (trait == 2) {
                ItemManager.locker[0] = new Item(name, 0, 0 , value);
            }
            for (int m = 0; m < ItemManager.locker.Length; m++) {
                if (ItemManager.locker[m] != null) {Debug.Log("Locker item " + m + ": " + ItemManager.locker[m].ToString());}
            }
        }
    }
}