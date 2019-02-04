using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using UnityEngine.UI;

#if !UNITY_EDITOR
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
    using System.Threading.Tasks;
#endif

public class DolphinManager : MonoBehaviour
{
    private Text text;
    private static string dolphinIpAddr = "192.168.0.125"; //177 per Phil 125 per il cartone
    private static string holoLensIpAddr = "192.168.0.147";
    private static int holoLensPort = 9000;
    private static string unityIpAddr = "192.168.0.173";
    private static int unityPort = 60000;
    private Stack<SamEvents> eventStack;

#if UNITY_EDITOR
    private HttpListener _listener;
#endif

#if !UNITY_EDITOR
        private StreamSocket socket;
        private static String serverIp = "192.168.0.140";
        private static int serverPort = 60000;
        private Task exchangeTask;
        private BinaryReader reader = null;

        private bool exchanging = false;
        private bool exchangeStopRequested = false;
        private string lastPacket = null;
#endif



    void Start()
    {
        eventStack = new Stack<SamEvents>();
        Debug.Log("Tentativo di connessione avviato!");

#if UNITY_EDITOR
        //Invoke("InitializeUnityServer", 4f);

#else
        Invoke("InitializeUWPServer", 4f);
#endif
    }

    void Update()
    {
        if (eventStack.Count != 0)
            HandleDolphinEvent(eventStack.Pop());
    }        


#if UNITY_EDITOR
    void InitializeUnityServer()
    {

        Invoke("connect", 4f);
        StartCoroutine(HttpMessage.SendSingleColorAllLeds(Color.yellow, dolphinIpAddr));

        
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://+:" + unityPort.ToString() + "/");

        _listener.Start();
        
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        
        
    }

    private void ListenerCallback(IAsyncResult result)
    {
        HttpListener listener = (HttpListener)result.AsyncState;
        HttpListenerContext context = listener.EndGetContext(result);

        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        string contRead = new StreamReader(request.InputStream).ReadToEnd();
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        SamEvents samEvents = new SamEvents();
        samEvents = JsonUtility.FromJson<SamEvents>(contRead);
        eventStack.Push(samEvents);
    }
#endif

#if !UNITY_EDITOR
    private async void InitializeUWPServer()
    {
        StartCoroutine(HttpMessage.SendHttpChange(serverIp, serverPort, dolphinIpAddr));
        Debug.Log("Delfino connesso!");

        try
            {        
                socket = new StreamSocket();
                HostName serverHost = new HostName(serverIp);
                await socket.ConnectAsync(serverHost, serverPort.ToString());              
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

#if !UNITY_EDITOR
    private async void ConnectUWP() 
    {
        try
        {        
            socket = new StreamSocket();
            HostName serverHost = new HostName(serverIp);
            await socket.ConnectAsync(serverHost, serverPort.ToString());              
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


                SamEvents samEvents = new SamEvents();
                samEvents = JsonUtility.FromJson<SamEvents>(message);
                eventStack.Push(samEvents);


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

    //call methods of InputHandler according to the message received from the dolphin
    private void HandleDolphinEvent(SamEvents samEvent)
    {
        if (samEvent.events[0].typ == "touch" && samEvent.events[0].act == true)
        {
            switch (samEvent.events[0].val)
            {
                case "1": InputHandler.OnNextColor();
                          StartCoroutine(HttpMessage.SendSingleColorAllLeds(InputHandler.CurrentColor, dolphinIpAddr));
                    break;
                case "2": InputHandler.OnPreviousColor();
                          StartCoroutine(HttpMessage.SendSingleColorAllLeds(InputHandler.CurrentColor, dolphinIpAddr));
                    break;
                case "5": InputHandler.SubmitColor();
                    break;
            }
        }
    }

}

