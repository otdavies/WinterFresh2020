using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Text;
using System.Net;
using System.Threading;

public class KnobInput : Observable<int[]>
{
    public int port = 7777;
    public string ipAddress = "";

    private UdpClient client;
    private IPEndPoint endPoint;
    private Thread listener;
    private Queue pQueue = Queue.Synchronized(new Queue()); // holds the packet queue

    private void Start()
    {
        endPoint = new IPEndPoint(Dns.GetHostAddresses(ipAddress)[0], port);
        client = new UdpClient(endPoint);
        Debug.Log(gameObject.name + " Listening for Data...");
        listener = new Thread(new ThreadStart(Translate));
        listener.IsBackground = true;
        listener.Start();
    }

    int[] val;
    private void Update()
    {
        lock (pQueue.SyncRoot)
        {
            if (pQueue.Count > 0)
            {
                val = (int[])pQueue.Dequeue();
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
            pQueue.Enqueue(Array.ConvertAll(str.Split(','), int.Parse));
        }
    }

    private void OnDisable()
    {
        client.Close();
        listener.Abort();
    }

}