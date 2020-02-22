using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Text;
using System.Net;
using System.Threading;
using System.Collections.Generic;

public class NetworkInput : Observable<NetworkIntMessage>
{
    public int port = 7777;
    public string ipAddress = "";

    private UdpClient client;
    private IPEndPoint endPoint;
    private Thread listener;
    private Queue pQueue = Queue.Synchronized(new Queue()); // holds the packet queue

    private Dictionary<string, int> idToType = new Dictionary<string, int>();

    private void Start()
    {
        endPoint = new IPEndPoint(Dns.GetHostAddresses(ipAddress)[0], port);
        client = new UdpClient(endPoint);
        Debug.Log(gameObject.name + " Listening for Data...");
        listener = new Thread(new ThreadStart(Translate));
        listener.IsBackground = true;
        listener.Start();

        // Setup conversion ids
        idToType.Add("S", 0);
        idToType.Add("C", 1);   
        idToType.Add("K", 2);
    }

    private void Update()
    {
        lock (pQueue.SyncRoot)
        {
            if (pQueue.Count > 0)
            {
                NetworkIntMessage val = (NetworkIntMessage)pQueue.Dequeue();
                Debug.Log((NetworkIntMessage.MessageType)val.MsgType + ": " + val.data[1]);
                OnChange(ref val);
            }
        }
    }

    private void Translate()
    {
        Byte[] data = new byte[0];
        while (true)
        {
            try
            {
                data = client.Receive(ref endPoint);
            }
            catch
            {
                Debug.Log("[Closed Connection]");
                client.Close();
                return;
            }

            string str = Encoding.ASCII.GetString(data);
            string[] split = str.Split(',');

            if(split.Length > 1 && idToType.ContainsKey(split[0]))
            {
                NetworkIntMessage.MessageType messageType = (NetworkIntMessage.MessageType)idToType[split[0]];
                split[0] = idToType[split[0]].ToString(); // Make sure we don't pass any strings forward
                
                NetworkIntMessage msg = new NetworkIntMessage(messageType, Array.ConvertAll(split, int.Parse));
                pQueue.Enqueue(msg);
            }
        }
    }

    private void OnDisable()
    {
        client.Close();
        listener.Abort();
    }

}