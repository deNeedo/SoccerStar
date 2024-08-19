using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
public class NetworkManager : MonoBehaviour {
    private TcpClient client;
    private NetworkStream stream;
    public void Connect() {
        try {
            client = new TcpClient("10.147.17.201", 54429);
            stream = client.GetStream();
            byte[] data = Encoding.UTF8.GetBytes("Testing connection\n");
            stream.Write(data, 0, data.Length);
        } catch (Exception e) {
            Debug.LogError("Socket error: " + e.Message);
        }
    }
    public void Update()
    {
        if (stream != null && stream.DataAvailable)
        {
            byte[] receivedData = new byte[client.Available];
            stream.Read(receivedData, 0, receivedData.Length);
            string message = Encoding.UTF8.GetString(receivedData);
            Debug.Log("Received: " + message);
        }
    }
    public void OnDestroy()
    {
        stream.Close();
        client.Close();
    }
}