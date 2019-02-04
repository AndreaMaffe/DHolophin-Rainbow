using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#if !UNITY_EDITOR
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
    using System.Threading.Tasks;
#endif

public class UWPSocketClient : MonoBehaviour {


#if !UNITY_EDITOR
        private StreamSocket socket;
        private String ip = "192.168.0.140";
        private String port = "60000";
        private Task exchangeTask;
        private BinaryReader reader = null;

        private bool exchanging = false;
        private bool exchangeStopRequested = false;
        private string lastPacket = null;
#endif

    void Start ()
    {
        //Invoke("Connect", 7f);
        //Debug.Log("Tentativo di connessione avviato!");
    }

  

    void Connect()
    {
#if !UNITY_EDITOR
        ConnectUWP();
#endif
    }

#if !UNITY_EDITOR
    private async void ConnectUWP() 
    {
        try
        {        
            socket = new StreamSocket();
            HostName serverHost = new HostName(ip);
            await socket.ConnectAsync(serverHost, port);              
            Stream streamIn = socket.InputStream.AsStreamForRead();
            reader = new BinaryReader(streamIn);          
            Debug.Log("Connesso al server!");
            RestartExchange();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
#endif

    public void ExchangePackets()
    {
#if !UNITY_EDITOR
        GameManager.stringa = "working";
        Debug.Log("Entro nel loop");
        try
        { 
            while (!exchangeStopRequested)
            {
                if (reader == null) continue;
                exchanging = true;

                byte[] received = reader.ReadBytes(100);
                String message = System.Text.Encoding.UTF8.GetString(received);
                Debug.Log("RECEIVED: " + message);
                exchanging = false;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

#endif
    }

    public void RestartExchange()
    {
#if !UNITY_EDITOR
        try
        { 
            if (exchangeTask != null) 
                StopExchange();
            exchangeStopRequested = false;
            exchangeTask = Task.Run(() => ExchangePackets());
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
#endif
    }


    public void StopExchange()
    {
#if !UNITY_EDITOR
        try
        { 
            exchangeStopRequested = true;
      
            if (exchangeTask != null) 
            {
                exchangeTask.Wait();
                socket.Dispose();
                reader.Dispose();
                socket = null;
                exchangeTask = null;
                reader = null;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
#endif
    }

}
