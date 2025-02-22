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
            Debug.LogError("Connection error: " + e.Message);
            return false;
        }
    }
    private static string[] AwaitResponse(string message) {
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
        while (server_response == null) {
            if (stream != null && stream.DataAvailable) {
                byte[] receivedData = new byte[client.Available];
                stream.Read(receivedData, 0, receivedData.Length);
                server_response = Encoding.UTF8.GetString(receivedData).Trim();
            }
            Thread.Sleep(1);
        }
        return server_response.Split(' ');
    }
    public static void Login(string username, string password) {
        if (Connect()) {
            string message = "LOGIN " + username + " " + password + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1].Trim() == "0") {
                PlayerManager.SetName(temp[2]);
                FetchEndurance(temp[2]);
                FetchCash(temp[2]);
                FetchLockerItems(temp[2]);
                FetchSessions(temp[2]);
                FetchStars(temp[2]);
                FetchStats(temp[2]);
                GameManager.ChangeScene("01_Profile");
            } else {
                Debug.Log("Wrong credentials");
            }
        }
    }
    public static void Register(string username, string password) {
        if (Connect()) {
            string message = "REGISTER " + username + " " + password + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1].Trim() == "0") {GameManager.ChangeScene("00_Login");}
        }
    }
    public static void FetchEndurance(string username) {
        if (Connect()) {
            string message = "FETCHENDURANCE " + username + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1].Trim() == "0") {
                PlayerManager.SetEndurance(int.Parse(temp[2]));
                Debug.Log("Endurance fetched successfully");
            } else {
                Debug.LogError("Failed to fetch endurance");
            }
        }
    }
    public static void FetchCash(string username) {
        if (Connect()) {
            string message = "FETCHCASH " + username + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1] == "0") {
                PlayerManager.SetCash(double.Parse(temp[2]));
                Debug.Log("Cash fetched successfully");
            } else {
                Debug.LogError("Failed to fetch cash");
            }
        }
    }
    public static void FetchLockerItems(string username) {
        if (Connect()) {
            string message = "FETCHLOCKERITEMS " + username + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            for (int i = 2; i < temp.Length; i++) {
                Item item = CreateItem(temp[i]);
                PlayerManager.SetLockerItem(i - 2, item);
            }
            Debug.Log("Locker items fetched successfully");
        }
        else {
            Debug.LogError("Failed to fetch cash");
        }
    }
    public static void FetchSessions(string username) {
        if (Connect()) {
            string message = "FETCHSESSIONS " + username + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1] == "0") {
                PlayerManager.SetSessions(int.Parse(temp[2]));
                Debug.Log("Sessions fetched successfully");
            } else {
                Debug.LogError("Failed to fetch sessions");
            }
        }
    }
    public static void FetchStars(string username) {
        if (Connect()) {
            string message = "FETCHSTARS " + username + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1] == "0") {
                PlayerManager.SetStars(int.Parse(temp[2]));
                Debug.Log("Stars fetched successfully");
            } else {
                Debug.LogError("Failed to fetch stars");
            }
        }
    }
    public static void FetchStats(string username) {
        if (Connect()) {
            string message = "FETCHSTATS " + username + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1].Trim() == "0") {
                temp = temp[2].Split("\t");
                PlayerManager.SetTrait(0, int.Parse(temp[0])); PlayerManager.SetTrait(1, int.Parse(temp[1])); PlayerManager.SetTrait(2, int.Parse(temp[2]));
                Debug.Log("Traits fetched successfully");
            } else {
                Debug.LogError("Failed to fetch stats.");
            }
        }
    }
    public static Item GenerateFood(string username) {
        if (Connect() == true) {
            string message = "GENERATE_FOOD_ITEM " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {AwaitResponse(message); Thread.Sleep(10);}
            message = server_response.Split(' ')[2]; server_response = null;
            return CreateItem(message);
        } else {return null;}
    }
    public static Item GenerateClothing(string username, int slot) {
        if (Connect() == true) {
            string message = "GENERATE_CLOTHING_ITEM " + username + " " + slot + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {AwaitResponse(message); Thread.Sleep(10);}
            message = server_response.Split(' ')[2]; server_response = null;
            return CreateItem(message);
        } else {return null;}
    }
    public static bool BuyClothing(string username, int slot) {
        if (Connect() == true) {
            string message = "BUY_CLOTHING_ITEM " + username + " " + slot + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {AwaitResponse(message); Thread.Sleep(10);}
            string[] response = server_response.Split(' '); server_response = null;

            if (response[1] == "0")
                return true;
            else
                return false;
                
        } else {return false;}
    }
    private static Item CreateItem(string data) {
        // Debug.Log(data);
        string[] temp = data.Split('_'); Item item = new(); int trait_id = 0;
        item.name = temp[1];
        item.price = double.Parse(temp[2]);

        for (int m = 4; m < temp.Length; m++) {
            switch (m % 2) {
                case 0:
                    trait_id = int.Parse(temp[m]);
                    break;
                case 1:
                    switch (trait_id) {
                        case 0: item.trait0 += int.Parse(temp[m]); break;
                        case 1: item.trait1 += int.Parse(temp[m]); break;
                        case 2: item.trait2 += int.Parse(temp[m]); break;
                    }
                    break;
            }
        }
        item.type = temp[0]; 
        return item;
    }
    public static void FetchClothes(string username) {
        bool flag = Connect();
        if (flag == true) {
            string message = "FETCH_CLOTHING_ITEMS " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {AwaitResponse(message); Thread.Sleep(10);}
            message = server_response.Split(' ')[2]; server_response = null;
            string[] message2 = message.Split('\n');
            for (int m = 0; m < message2.Length; m++) {
                ItemManager.SetClothing(CreateItem(message2[m]), m);
            }
        }
    }
    public static void FetchFood(string username) {
        bool flag = Connect();
        if (flag == true) {
            string message = "FETCH_FOOD_ITEM " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            while (server_response == null) {AwaitResponse(message); Thread.Sleep(10);}
            message = server_response.Split(' ')[2]; server_response = null;
            ItemManager.SetFood(CreateItem(message));
        }
    }
    public static bool UseRelaxSession(string username) {
        if (Connect() == true) {
            string message = "USERELAX " + username + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);

            while (server_response == null) {
                AwaitResponse(message);
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
            } else {
                Debug.LogError("Failed to use relax session on the server.");
                return false;
            }
        }
        return false;
    }

    public static bool StartWork(int hours) {
        if (Connect()) {
            string message = "STARTWORK " + PlayerManager.GetName().Trim() + " " + hours + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1].Trim() == "0") {
                Debug.Log("Work started successfully.");
                return true;
            }
            else {
                Debug.LogError("Failed to start work.");
                return false;
            }
        }
        return false;
    }
    public static bool CancelWork() {
        if (Connect()) {
            string message = "CANCELWORK " + PlayerManager.GetName() + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1].Trim() == "0") {
                PlayerManager.SetEndTimeStr("");
                PlayerManager.SetStartTimeStr("");
                // Debug.Log("Work canceled successfully.");
                return true;
            } else {
                Debug.LogError("Failed to cancel work.");
                return false;
            }
        }
        return false;
    }
    public static void CheckWorkCompletion() {
        if (Connect()) {
            string message = "CHECKWORK " + PlayerManager.GetName() + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1].Trim() == "0") {
                double cash = double.Parse(temp[2]);
                PlayerManager.SetCash(cash);
                PlayerManager.SetEndTimeStr("");
                PlayerManager.SetStartTimeStr("");
                Debug.Log("Work completed, cash added.");
            }
            else {
                string startTimeStr;
                string endTimeStr;
                if (temp.Length > 4) {
                    startTimeStr = temp[2] + " " + temp[3];
                    endTimeStr = temp[4] + " " + temp[5];
                } else {
                    startTimeStr = "";
                    endTimeStr = "";
                }
                PlayerManager.SetStartTimeStr(startTimeStr);
                PlayerManager.SetEndTimeStr(endTimeStr);
            }
        } else Debug.Log("Connection Error");
    }
    public static void FetchTraining(string username) {
        if (Connect()) {
            string message = "FETCHTRAINING " + username + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1] == "0") {
                if (temp.Length > 10) {
                    StringBuilder trainingInfo = new();
                    TrainingType[] trainings = new TrainingType[3];
                    for (int j = 0, i = 2; i < temp.Length; i += 4) {
                        string title = temp[i].Replace("|", " ");
                        string description = temp[i + 1].Replace("|", " ");
                        string trait = temp[i + 2];
                        string endTimeStr = temp[i + 3];
                        float.TryParse(endTimeStr, out float duration);
                        trainings[j] = new TrainingType(title, description, duration) {Trait = trait};
                        trainingInfo.AppendLine($"Title: {title}, Description: {description}, Trait: {trait}, Duration: {duration}");
                        j++;
                    }
                    PlayerManager.SetAvailableTrainings(trainings);
                    // Debug.Log("Fetched Training Info:\n" + trainingInfo.ToString());
                } else {
                    string trainingTitle = temp[2].Replace("|", " ");
                    string trainingDescription = temp[3].Replace("|", " ");
                    string durationStr = temp[4];
                    string endTimeStr = temp[5].Replace("|", " ");
                    float.TryParse(durationStr, out float duration);
                    TrainingType currentTraining = new(trainingTitle, trainingDescription, duration) {TrainingEndTime = endTimeStr};
                    PlayerManager.SetCurrentTraining(currentTraining);
                    // Debug.Log($"Ongoing Training Info:\nTitle: {trainingTitle}, Description: {trainingDescription}, Duration: {durationStr}, End Time: {endTimeStr}");
                }
            }
            else {
                Debug.LogError("Failed to fetch training.");
            }
        }
        else {
            Debug.LogError("Connection error. Could not fetch training.");
        }
    }

    public static bool StartTraining(string username, int trainingNumber) {
        if (Connect()) {
            string message = "STARTTRAINING " + username + " " + trainingNumber + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1].Trim() == "0") {
                Debug.Log("Training started successfully.");
                return true;
            } else {
                Debug.LogError("Failed to start training.");
                return false;
            }
        }
        return false;
    }

    public static bool StopTraining(string username) {
        if (Connect()) {
            string message = "STOPTRAINING " + username + "\n";
            string[] temp = AwaitResponse(message); server_response = null;
            if (temp[1].Trim() == "0") {
                Debug.Log("Training stopped successfully.");
                return true;
            } else {
                Debug.LogError("Failed to stop training.");
                return false;
            }
        } else {
            Debug.LogError("Couldn't connect.");
            return false;
        }
    }
}