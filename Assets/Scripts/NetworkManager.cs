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
    private static readonly string configFileName = "App.config";
    private static string server_ip;
    private static int server_port;
    private static string server_response = null;
    private static void ResponseCheck() {
        if (stream != null && stream.DataAvailable) {
            byte[] receivedData = new byte[client.Available];
            stream.Read(receivedData, 0, receivedData.Length);
            server_response = Encoding.UTF8.GetString(receivedData);
        }
    }
    private static void ConnectInit() {
        string path = Path.Combine(Application.streamingAssetsPath, configFileName);
        if (File.Exists(path)) {
            try {
                XmlDocument xmlDoc = new(); xmlDoc.Load(path);
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
            if (server_ip == null) {ConnectInit();}
            client = new TcpClient(server_ip, server_port);
            stream = client.GetStream();
            return true;
        } catch (Exception e) {
            Debug.LogError("Socket error: " + e.Message);
            return false;
        }
    }
    public static void Login(string username, string password) {
        bool flag = Connect();
        if (flag == true) {
            string message = "LOGIN " + username + " " + password + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {ResponseCheck(); Thread.Sleep(10);}
            string[] temp = server_response.Split(' ');
            server_response = null;
            if (temp[1] == "0") {
                FetchTraits(temp[2]);
                FetchStars(temp[2]);
                FetchEndurance(temp[2]);
                FetchSessions(temp[2]);
                // FetchLockerItems(temp[2]);
                PlayerManager.SetName(temp[2]);
                GameManager.ChangeScene("01_Profile");
            }
        }
    }
    public static void Register(string username, string password) {
        bool flag = Connect();
        if (flag == true) {
            string message = "REGISTER " + username + " " + password + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {ResponseCheck(); Thread.Sleep(10);}
            string[] temp = server_response.Split(' ');
            server_response = null;
            if (temp[1].Trim() == "0") { // Zmieniłem na Trim bo za chuja mi nie działało bez, nie mam pojęcia czemu
                GameManager.ChangeScene("00_Login");
            }
        }
    }
    public static void FetchTraits(string username) {
        bool flag = Connect();
        if (flag == true) {
            string message = "FETCHSTATS " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {ResponseCheck(); Thread.Sleep(10);}
            string[] temp = server_response.Split(' ');
            server_response = null;
            if (temp[1] == "0") {
                temp = temp[2].Split('\t');
                PlayerManager.SetTrait(0, int.Parse(temp[0])); PlayerManager.SetTrait(1, int.Parse(temp[1])); PlayerManager.SetTrait(2, int.Parse(temp[2]));
            }
        }
    }
    public static void FetchStars(string username) {
        bool flag = Connect();
        if (flag == true) {
            string message = "FETCHSTARS " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {ResponseCheck(); Thread.Sleep(10);}
            string[] temp = server_response.Split(' ');
            server_response = null;
            if (temp[1] == "0") {
                PlayerManager.SetStars(int.Parse(temp[2]));
            }
        }
    }
    public static void FetchEndurance(string username) {
        bool flag = Connect();
        if (flag == true) {
            string message = "FETCHENDURANCE " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {ResponseCheck(); Thread.Sleep(10);}
            string[] temp = server_response.Split(' ');
            server_response = null;
            if (temp[1] == "0") {
                PlayerManager.SetEndurance(int.Parse(temp[2]));
            }
        }
    }
    public static void FetchSessions(string username) {
        bool flag = Connect();
        if (flag == true) {
            string message = "FETCHSESSIONS " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {ResponseCheck(); Thread.Sleep(10);}
            string[] temp = server_response.Split(' ');
            server_response = null;
            if (temp[1] == "0") {
                PlayerManager.SetSessions(int.Parse(temp[2]));
            }
        }
    }
    public static Item GenerateFood(string username) {
        if (Connect() == true) {
            string message = "GENERATE_FOOD_ITEM " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {ResponseCheck(); Thread.Sleep(10);}
            message = server_response.Split(' ')[2]; server_response = null;
            return new();
            // string name = temp[0]; int trait = int.Parse(temp[1].Split(',')[0]); int value = int.Parse(temp[1].Split(',')[1]);
            // if (trait == 0) {
            //     PlayerManager.SetLockerItem(0, new Item(name, value, 0, 0));
            // } else if (trait == 1) {
            //     PlayerManager.SetLockerItem(0, new Item(name, 0, value, 0));
            // } else if (trait == 2) {
            //     PlayerManager.SetLockerItem(0, new Item(name, 0, 0, value));
            // }
            // for (int m = 0; m < 4; m++) {
            //     if (PlayerManager.GetLockerItem(m) != null) {Debug.Log("Locker item " + m + ": " + PlayerManager.GetLockerItem(m).ToString());}
            // }
        } else {return null;}
    }
    public static Item GenerateClothing(string username) {
        if (Connect() == true) {
            string message = "GENERATE_CLOTHING_ITEM " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {ResponseCheck(); Thread.Sleep(10);}
            message = server_response.Split(' ')[2]; server_response = null;
            string[] temp = message.Split('_'); Item item = new(); int trait_id = 0;
            for (int m = 1; m < temp.Length; m++) {
                switch (m % 2) {
                    case 0:
                        if (m == 2) {continue;}
                        else {
                            switch (trait_id) {
                                case 0: item.trait0 += int.Parse(temp[m]); break;
                                case 1: item.trait1 += int.Parse(temp[m]); break;
                                case 2: item.trait2 += int.Parse(temp[m]); break;
                            }
                        }
                        break;
                    case 1:
                        if (m == 1) {item.name = temp[m];}
                        else {trait_id = int.Parse(temp[m]);}
                        break;
                }
            }
            return item;
        } else {return null;}
    }
    public static bool UseRelaxSession(string username)
    {
        bool flag = Connect();
        if (flag == true) {
            string message = "UseRelax " + username;
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);

            while (server_response == null)
            {
                ResponseCheck();
                Thread.Sleep(10);
            }

            string[] response = server_response.Split(' ');
            server_response = null;

            if (response[1] == "0") {
                int newEndurance = int.Parse(response[2]);
                int newStars = int.Parse(response[3]);
                int newSessions = int.Parse(response[4]);

                PlayerManager.SetEndurance(newEndurance);
                PlayerManager.SetStars(newStars);
                PlayerManager.SetSessions(newSessions);

                Debug.Log("Relax session used successfully. Endurance increased, and stars/sessions decreased.");
                return true;
            }
            else
            {
                Debug.LogError("Failed to use relax session on the server.");
                return false;
            }
        }
        return false;
    }
    public static void FetchLockerItems(string username) {
        bool flag = Connect();
        if (flag == true) {
            string message = "FETCHLOCKER " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {ResponseCheck(); Thread.Sleep(10);}
            string[] temp = server_response.Split(' ');
            server_response = null;
            if (temp[1] == "0") {
                
            }
        }
    }
}