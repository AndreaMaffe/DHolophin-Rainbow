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

    private Text text;

#if !UNITY_EDITOR
        private StreamSocket socket;
        private String ip = "192.168.0.140";
        private String port = "60000";
        private Task exchangeTask;
        private StreamReader reader = null;

        private bool exchanging = false;
        private bool exchangeStopRequested = false;
        private string lastPacket = null;
#endif

    void Start ()
    {
        text = GameObject.Find("DEBUG_TEXT").GetComponent<Text>();
        Invoke("Connect", 7f);		
	}

    void Update()
    {

        text.text = GameManager.stringa;


#if !UNITY_EDITOR

        /*
        if (reader != null)
        {
            try
            {
                string messageReceived;
                messageReceived = reader.ReadLine();
                text.text = messageReceived;
            }
            catch (Exception e)
            {
                text.text = e.ToString();
            }
        }
        */
#endif
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
            reader = new StreamReader(streamIn);          
            text.text = "Connesso al server!";
            RestartExchange();
        }
        catch (Exception e)
        {
            text.text = e.ToString();
        }
    }
#endif

    public void ExchangePackets()
    {
#if !UNITY_EDITOR
        text.text = "In attesa di messaggi...";
        GameManager.stringa = "working";

        try
        { 
            while (!exchangeStopRequested)
            {
                if (reader == null) continue;
                exchanging = true;

                string received = null;
                received = reader.ReadLine();
                text.text = received;

                exchanging = false;
            }
        }
        catch (Exception e)
        {
            text.text = e.ToString();
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
            text.text = e.ToString();
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
            text.text = e.ToString();
        }
#endif
    }

}
