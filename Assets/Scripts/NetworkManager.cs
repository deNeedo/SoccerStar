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
                NetworkManager.FetchCash(temp[2]);
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
            Debug.Log("temp: " + temp[1]);
            NetworkManager.server_response = null;
            if (temp[1].Trim() == "0") { // Zmieniłem na Trim bo za chuja mi nie działało bez, nie mam pojęcia czemu
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
            } else {
                Debug.LogError("Failed to fetch stats.");
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
            } else {
                Debug.LogError("Failed to fetch stars.");
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
            } else {
                Debug.LogError("Failed to fetch endurance.");
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
            } else {
                Debug.LogError("Failed to fetch sessions.");
            }
        }
    }
    public static void FetchCash(string username) {
            bool flag = NetworkManager.Connect();
            if (flag == true)
            {
                string message = "FETCHCASH " + username + "\n";
                byte[] data = Encoding.UTF8.GetBytes(message);
                NetworkManager.stream.Write(data, 0, data.Length);
                while (NetworkManager.server_response == null){NetworkManager.ResponseCheck();Thread.Sleep(10);}

                string[] response = (NetworkManager.server_response).Split(' ');
                NetworkManager.server_response = null;

                if (response[1] == "0")
                {
                    double cash = double.Parse(response[2]);
                    PlayerManager.SetCash(cash);
                    Debug.Log("Cash fetched successfully.");
                } else {
                    Debug.LogError("Failed to fetch cash.");
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

    public static bool UseRelaxSession(string username) {
        bool flag = NetworkManager.Connect();
        if (flag == true) {
            string message = "UseRelax " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);

            while (NetworkManager.server_response == null) {
                NetworkManager.ResponseCheck();
                Thread.Sleep(10);
            }

            string[] response = NetworkManager.server_response.Split(' ');
            NetworkManager.server_response = null;

            if (response[1] == "0") {
                int newEndurance = int.Parse(response[2]);
                int newStars = int.Parse(response[3]);
                int newSessions = int.Parse(response[4]);

                PlayerManager.SetEndurance(newEndurance);
                PlayerManager.SetStars(newStars);
                PlayerManager.SetSessions(newSessions);

                Debug.Log("Relax session used successfully. Endurance increased, and stars/sessions decreased.");
                return true;
            } else {
                Debug.LogError("Failed to use relax session on the server.");
                return false;
            }
        }
        return false;
    }

    public static bool StartWork(int hours) {
        bool flag = NetworkManager.Connect();
        if (flag) {
            string message = "STARTWORK " + PlayerManager.Get().Trim() + " " + hours + "\n";

            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);

            while (NetworkManager.server_response == null) {
                NetworkManager.ResponseCheck();
                Thread.Sleep(10);
            }

            string[] response = NetworkManager.server_response.Split(' ');
            NetworkManager.server_response = null;

            if (response[1].Trim() == "0") { // popierdoli mnie, znowu trim, jebac
                // Debug.Log("Work started successfully.");
                return true;
            }
            else {
                Debug.LogError("Failed to start work.");
                return false;
            }
        }
        Debug.LogError("Couldn't connect.");
        return false;
    }

    public static bool CancelWork() {
        bool flag = NetworkManager.Connect();
        if (flag) {
            string message = "CANCELWORK " + PlayerManager.Get() + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);

            while (NetworkManager.server_response == null) {
                NetworkManager.ResponseCheck();
                Thread.Sleep(10);
            }

            string[] response = NetworkManager.server_response.Split(' ');
            NetworkManager.server_response = null;

            Debug.Log("response: " + response[0] + " " + response[1]);
            if (response[1].Trim() == "0") {
                // Debug.Log("Work canceled successfully.");
                return true;
            } else {
                Debug.LogError("Failed to cancel work.");
                return false;
            }
        }
        else {
            Debug.Log("Connection Error");
            return false;
        }
    }
    public static void CheckWorkCompletion() {
        bool flag = NetworkManager.Connect();
        if (flag) {
            string message = "CHECKWORK " + PlayerManager.Get() + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkManager.stream.Write(data, 0, data.Length);

            while (NetworkManager.server_response == null) {
                NetworkManager.ResponseCheck();
                Thread.Sleep(10);
            }

            string[] response = NetworkManager.server_response.Split(' ');
            NetworkManager.server_response = null;

            if (response[1] == "0") {
                double cash = double.Parse(response[2]);
                PlayerManager.SetCash(cash);
                PlayerManager.SetEndTimeStr("");
                PlayerManager.SetStartTimeStr("");
                Debug.Log("Work completed, cash added.");
            }
            else {
                string startTimeStr;
                string endTimeStr;
                if (response.Length > 4) {
                    startTimeStr = response[2] + " " + response[3];
                    endTimeStr = response[4] + " " + response[5];
                } else {
                    startTimeStr = "";
                    endTimeStr = "";
                }
                PlayerManager.SetStartTimeStr(startTimeStr);
                PlayerManager.SetEndTimeStr(endTimeStr);
            }
        }
        else Debug.Log("Connection Error");
    }





}